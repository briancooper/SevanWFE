using Newtonsoft.Json;
using OptimaJet.Workflow.Core.Model;
using OptimaJet.Workflow.Core.Runtime;
using Workflow.Abstractions.Database;
using Workflow.Core.Models;
using Workflow.Core.ServiceLocator;

namespace Workflow.Engine.Services
{
    public partial class ActionService
    {
        public static dynamic GetNamespace(ProcessInstance process, WorkflowRuntime runtime, string parameters)
        {
            var captureWorkflow = ServiceLocator.Resolve<IRepository<WorkflowAssociation>>().FirstOrDefault(x => x.Id == process.ProcessId);
            return captureWorkflow != null ? JsonConvert.DeserializeObject(captureWorkflow.Entity) : null;
        }
    }
}
