using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OptimaJet.Workflow.Core.Model;
using OptimaJet.Workflow.Core.Runtime;
using Workflow.Abstractions.Database;
using Workflow.Abstractions.Services;
using Workflow.Core.Models;
using Workflow.Engine.Services.Action.Utils;
using Workflow.Core.Models.Templates;

namespace Workflow.Engine.Services
{
    public partial class ActionService : IActionService
    {
        private readonly Dictionary<string, Action<ProcessInstance, WorkflowRuntime, string, JObject>> _actions =
              new Dictionary<string, Action<ProcessInstance, WorkflowRuntime, string, JObject>>();

        private readonly Dictionary<string, Func<ProcessInstance, WorkflowRuntime, string, CancellationToken, JObject, Task>> _asyncActions =
                new Dictionary<string, Func<ProcessInstance, WorkflowRuntime, string, CancellationToken, JObject, Task>>();

        private readonly Dictionary<string, Func<ProcessInstance, WorkflowRuntime, string, JObject, bool>> _conditions =
            new Dictionary<string, Func<ProcessInstance, WorkflowRuntime, string, JObject, bool>>();

        private readonly
            Dictionary<string, Func<ProcessInstance, WorkflowRuntime, string, CancellationToken, JObject, Task<bool>>> _asyncConditions =
                new Dictionary<string, Func<ProcessInstance, WorkflowRuntime, string, CancellationToken, JObject, Task<bool>>>();

        private readonly IRepository<WorkflowAssociation> _workflowAssociationsRepository;

        private readonly IRepository<Template> _templatesRepository;

        private readonly IRepository<FormTemplate> _formTemplateRepository;

        private readonly IEmailService _emailService;

        private readonly IConverterService _converterService;

        private readonly IStorageService _storageService;


        public ActionService(IRepository<WorkflowAssociation> workflowAssociationsRepository, IRepository<Template> templatesRepository, IEmailService emailService, IConverterService converterService, IStorageService storageService, IRepository<FormTemplate> formTemplateRepository)
        {
            _workflowAssociationsRepository = workflowAssociationsRepository;

            _templatesRepository = templatesRepository;


            _emailService = emailService;

            _converterService = converterService;

            _storageService = storageService;

            _formTemplateRepository = formTemplateRepository;


            _asyncActions.Add(nameof(LoadTemplate), LoadTemplate);

            _asyncActions.Add(nameof(TemplateToPdf), TemplateToPdf);

            _asyncActions.Add(nameof(SendEmail), SendEmail);

            _asyncActions.Add(nameof(AmazonS3UploadFile), AmazonS3UploadFile);

            _asyncActions.Add(nameof(GetEntity), GetEntity);

            _asyncActions.Add(nameof(OnErrorSendEmail), OnErrorSendEmail);

            _asyncActions.Add(nameof(ApiRequestAsync), ApiRequestAsync);

            _asyncActions.Add(nameof(LoadFormTemplatePath), LoadFormTemplatePath);



            _actions.Add(nameof(Start), Start);

            _actions.Add(nameof(End), End);

            _actions.Add(nameof(FillTemplate), FillTemplate);

            _actions.Add(nameof(SetAttribute), SetAttribute);

            _actions.Add(nameof(FillPdfForm), FillPdfForm);

            _actions.Add(nameof(ApiRequest), ApiRequest);

            _actions.Add(nameof(SetEntityAttribute), SetEntityAttribute);

            _actions.Add(nameof(UpdateEntityAttribute), UpdateEntityAttribute);

            _actions.Add(nameof(RemoveEntityAttribute), RemoveEntityAttribute);

            _actions.Add(nameof(SetUserRoles), SetUserRoles);

            _actions.Add(nameof(RemoveUserRoles), RemoveUserRoles);

            _actions.Add(nameof(UpdateUserRoles), UpdateUserRoles);


            _conditions.Add(nameof(IsAnyErrorOccurred), IsAnyErrorOccurred);

            _conditions.Add(nameof(IsCurrentErrorOccurred), IsCurrentErrorOccurred);

            _conditions.Add(nameof(IsUserCertified), IsUserCertified);
        }

        public virtual void AddAction(string actionName, Func<ProcessInstance, WorkflowRuntime, string, CancellationToken, JObject, Task> actionMethod)
        {
            _asyncActions.Add(actionName, actionMethod);
        }

        public virtual void ExecuteAction(string name, ProcessInstance processInstance, WorkflowRuntime runtime, string actionParameter)
        {
            if (_actions.ContainsKey(name))
            {
                var workflowAssociation = _workflowAssociationsRepository.FirstOrDefault(x => x.Id == processInstance.ProcessId);

                if (workflowAssociation == null)
                {
                    throw new NotImplementedException($"Association {processInstance.ProcessId} not found.");
                }

                if (string.IsNullOrWhiteSpace(workflowAssociation.Entity))
                {
                    workflowAssociation.Entity = "{}";
                }

                var entity = JObject.Parse(workflowAssociation.Entity);

                try
                {
                    _actions[name].Invoke(processInstance, runtime, actionParameter, entity);
                }
                catch (Exception exception)
                {
                    Helpers.ActionErrorHandling(processInstance.ExecutedActivity.Name, name, exception, entity);
                }

                workflowAssociation.Entity = JsonConvert.SerializeObject(entity);

                _workflowAssociationsRepository.Update(workflowAssociation);
            }
            else
            {
                throw new NotImplementedException($"Async Action with name {name} isn't implemented");
            }
        }

        public virtual async Task ExecuteActionAsync(string name, ProcessInstance processInstance, WorkflowRuntime runtime, string actionParameter, CancellationToken token)
        {
            if (_asyncActions.ContainsKey(name))
            {
                var workflowAssociation = await _workflowAssociationsRepository.FirstOrDefaultAsync(x => x.Id == processInstance.ProcessId);

                if (workflowAssociation == null)
                {
                    throw new NotImplementedException($"Association {processInstance.ProcessId} not found.");
                }

                if (string.IsNullOrWhiteSpace(workflowAssociation.Entity))
                {
                    workflowAssociation.Entity = "{}";
                }

                var entity = JObject.Parse(workflowAssociation.Entity);

                try
                {
                    await _asyncActions[name].Invoke(processInstance, runtime, actionParameter, token, entity);
                }
                catch (Exception exception)
                {
                    Helpers.ActionErrorHandling(processInstance.ExecutedActivity.Name, name, exception, entity);
                }

                workflowAssociation.Entity = JsonConvert.SerializeObject(entity);

                await _workflowAssociationsRepository.UpdateAsync(workflowAssociation);
            }
            else
            {
                throw new NotImplementedException($"Async Action with name {name} isn't implemented");
            }
        }

        public virtual bool ExecuteCondition(string name, ProcessInstance processInstance, WorkflowRuntime runtime, string actionParameter)
        {
            if (_conditions.ContainsKey(name))
            {
                var workflowAssociation = _workflowAssociationsRepository.FirstOrDefault(x => x.Id == processInstance.ProcessId);

                if (workflowAssociation == null)
                {
                    throw new NotImplementedException($"Association {processInstance.ProcessId} not found.");
                }

                if (string.IsNullOrWhiteSpace(workflowAssociation.Entity))
                {
                    workflowAssociation.Entity = "{}";
                }

                var entity = JObject.Parse(workflowAssociation.Entity);

                var condition = _conditions[name].Invoke(processInstance, runtime, actionParameter, entity);

                workflowAssociation.Entity = JsonConvert.SerializeObject(entity);

                _workflowAssociationsRepository.Update(workflowAssociation);

                return condition;
            }
            throw new NotImplementedException($"Condition with name {name} isn't implemented");
        }

        public virtual async Task<bool> ExecuteConditionAsync(string name, ProcessInstance processInstance, WorkflowRuntime runtime, string actionParameter, CancellationToken token)
        {
            if (_asyncConditions.ContainsKey(name))
            {
                var workflowAssociation = await _workflowAssociationsRepository.FirstOrDefaultAsync(x => x.Id == processInstance.ProcessId);

                if (workflowAssociation == null)
                {
                    throw new NotImplementedException($"Association {processInstance.ProcessId} not found.");
                }

                if (string.IsNullOrWhiteSpace(workflowAssociation.Entity))
                {
                    workflowAssociation.Entity = "{}";
                }

                var entity = JObject.Parse(workflowAssociation.Entity);

                var condition =  await _asyncConditions[name].Invoke(processInstance, runtime, actionParameter, token, entity);

                workflowAssociation.Entity = JsonConvert.SerializeObject(entity);

                await _workflowAssociationsRepository.UpdateAsync(workflowAssociation);

                return condition;
            }
            throw new NotImplementedException($"Async Condition with name {name} isn't implemented");
        }

        public virtual bool IsActionAsync(string name)
        {
            return _asyncActions.ContainsKey(name);
        }

        public virtual bool IsConditionAsync(string name)
        {
            return _asyncConditions.ContainsKey(name);
        }

        public virtual List<string> GetActions()
        {
            return _actions.Keys.Union(_asyncActions.Keys).ToList();
        }

        public virtual List<string> GetConditions()
        {
            return _conditions.Keys.Union(_asyncConditions.Keys).ToList();
        }
    }
}
