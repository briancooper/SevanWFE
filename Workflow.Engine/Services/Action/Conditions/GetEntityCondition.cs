using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using OptimaJet.Workflow.Core.Model;
using OptimaJet.Workflow.Core.Runtime;
using System.Threading.Tasks;
using System.Threading;

namespace Workflow.Engine.Services
{
    public partial class ActionService
    {
        private async Task<bool> GetEntityCondition(ProcessInstance process, WorkflowRuntime runtime, string parameters, CancellationToken cancellationToken, JObject entity)
        {
            await Task.Delay(2000);

            //var captureWorkflow = _workflowAssociationRepository.FirstOrDefault(x => x.Id == process.ProcessId);
            //dynamic entity = JsonConvert.DeserializeObject(captureWorkflow.Entity);
            //JObject jo = JObject.FromObject(entity);
            //try
            //{
            //    var input = JsonConvert.DeserializeObject<EntityModel>(parameters);
            //    var entityType = AppDomain.CurrentDomain
            //        .GetAssemblies()
            //        .SelectMany(t => t.GetTypes()).FirstOrDefault(t => t.IsClass && t.Name == input.EntityType);

            //    var genericClass = typeof(IRepository<,>);
            //    var constructedClass = genericClass.MakeGenericType(entityType, typeof(long));
            //    dynamic repository = IocManager.Instance.Resolve(constructedClass);
            //    var id = FindAutoMapExpression(input.EntityId, jo);
            //    var result = repository.Get(int.Parse(id));

            //    jo.Add(input.AttributeName, JToken.FromObject(result, new JsonSerializer { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
            //    captureWorkflow.Entity = JsonConvert.SerializeObject(jo); // entity
            //    _workflowAssociationRepository.Update(captureWorkflow);
            //}
            //catch (Exception exception)
            //{
            //    jo.Add(OnErrorAttribute, JToken.FromObject(exception, new JsonSerializer
            //    {
            //        Formatting = Formatting.Indented,
            //        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            //        ContractResolver = new DefaultContractResolver()
            //        {
            //            IgnoreSerializableInterface = true
            //        }
            //    }));
            //    captureWorkflow.Entity = JsonConvert.SerializeObject(jo); // entity
            //    _workflowAssociationRepository.Update(captureWorkflow);
            //    return false;
            //}
            return true;
        }
    }
}
