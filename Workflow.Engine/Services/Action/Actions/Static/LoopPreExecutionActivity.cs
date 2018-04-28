using Newtonsoft.Json.Linq;
using OptimaJet.Workflow.Core.Model;
using OptimaJet.Workflow.Core.Runtime;

namespace Workflow.Engine.Services
{
    public partial class ActionService
    {
        public static void LoopPreExecutionActivity(ProcessInstance process, WorkflowRuntime runtime, string parameters, JObject entity)
        {
            //process.ExecutedActivity.PreExecutionImplementation.ForEach(x =>
            //{
            //    runtime.ActionProvider.ExecuteAction(x.ActionName, process, runtime,
            //        x.ActionParameter.IsNullOrEmpty() ? parameters : x.ActionParameter);
            //});
        }
    }
}
