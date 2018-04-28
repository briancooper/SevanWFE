using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using OptimaJet.Workflow.Core.Model;
using OptimaJet.Workflow.Core.Runtime;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Workflow.Engine.Services
{
    public partial class ActionService
    {
        private async Task GetEntity(ProcessInstance process, WorkflowRuntime runtime, string parameters, CancellationToken cancellationToken, JObject entity)
        {
            await Task.Delay(2000);

            //if (string.IsNullOrWhiteSpace(parameters))
            //{
            //    return;
            //}

            //entity["TemplateIterator"] = _workflowRawSql.Select(parameters);
        }
    }
}
