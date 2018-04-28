using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using OptimaJet.Workflow.Core.Model;
using OptimaJet.Workflow.Core.Runtime;
using System.Threading.Tasks;
using System.Threading;
using Workflow.Engine.Services.Action.Utils;

namespace Workflow.Engine.Services
{
    public partial class ActionService
    {
        private bool IsCurrentErrorOccurred(ProcessInstance process, WorkflowRuntime runtime, string parameters, JObject entity)
        {
            var activityName = process.CurrentActivityName;
            return entity.SelectToken(EngineConstants.ExecuteActionException) != null && entity.SelectToken(EngineConstants.ExecuteActionException).Any(e => e["ActivityName"].ToString() == activityName);
        }
    }
}
