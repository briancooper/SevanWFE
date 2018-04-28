using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.Abstractions.Database;
using Workflow.Core.Models.Triggers;
using Workflow.Application.Controllers.Triggers.Dto;
using Microsoft.AspNetCore.Authorization;
using Workflow.Core.Security;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Newtonsoft.Json.Linq;
using Workflow.Application.Utils;
using Workflow.Abstractions.Services;
using Workflow.Core.Models;
using System;

namespace Workflow.Application.Controllers
{
    [Authorize(Roles = Roles.Project)]
    public class TriggersController : BaseController
    {
        private readonly IRepository<ProjectTrigger> _projectTriggerRepository;

        private readonly IRepository<WorkflowScheme> _workflowSchemesRepository;

        private readonly IRepository<WorkflowAssociation> _workflowAssociationsRepository;

        private readonly IRepository<Trigger> _triggerRepository;

        private readonly IEngineService _workflowService;


        public TriggersController(IRepository<ProjectTrigger> projectTriggerRepository, IRepository<WorkflowScheme> workflowSchemesRepository, IRepository<WorkflowAssociation> workflowAssociationsRepository, IEngineService workflowService,
            IRepository<Trigger> triggerRepository)
        {
            _projectTriggerRepository = projectTriggerRepository;

            _workflowSchemesRepository = workflowSchemesRepository;

            _workflowAssociationsRepository = workflowAssociationsRepository;

            _triggerRepository = triggerRepository;

            _workflowService = workflowService;
        }

        [HttpGet]
        public async Task<List<ProjectTriggerDto>> GetAll()
        {
            var projectTriggers = await _projectTriggerRepository.Queryable().Include(pt => pt.Trigger).ToListAsync();

            return Mapper.Map<List<ProjectTriggerDto>>(projectTriggers.Select(pt => pt.Trigger));
        }

        [HttpPost]
        public async Task Fire(string name, [FromBody] JObject parameters)
        {
            var projectTrigger = await _projectTriggerRepository.Queryable().Include(pt => pt.Trigger).FirstOrDefaultAsync(pt => pt.Trigger.Name == name);

            if (projectTrigger == null)
            {
                throw new InvalidInputException(@"Trigger was not found.");
            }

            var schemes = await _workflowSchemesRepository.ToListAsync(ws => ws.TriggerId == projectTrigger.TriggerId);


            foreach (var scheme in schemes)
            {
                var workflowAssociation = new WorkflowAssociation
                {
                    //Id = Guid.NewGuid(),

                    SchemeCode = scheme.Code,

                    Entity = Convert.ToString(parameters)
                };

                await _workflowAssociationsRepository.InsertAsync(workflowAssociation);

                await _workflowService.RunSchemeAsync(scheme.Code, workflowAssociation.Id);
            }
        }

        [HttpPost]
        public async Task AssociateTriggerWithSchems(string triggerName, string schemeCode)
        {
            if (string.IsNullOrWhiteSpace(triggerName))
            {
                throw new InvalidInputException(@"The name of the trigger is empty.");
            }

            var trigger = await _triggerRepository.FirstOrDefaultAsync(p => p.Name == triggerName);

            if (trigger == null)
            {
                throw new InvalidInputException($@"trigger {triggerName} not found");
            }

            if (string.IsNullOrWhiteSpace(schemeCode))
            {
                throw new InvalidInputException(@"The name of the schema is empty.");
            }

            var schema = await _workflowSchemesRepository.FirstOrDefaultAsync(p => p.Code == schemeCode);

            if (schema == null)
            {
                throw new InvalidInputException($@"schema {schemeCode} not found");
            }

            schema.TriggerId = trigger.Id;

            await _workflowSchemesRepository.UpdateAsync(schema);
        }
    }
}
