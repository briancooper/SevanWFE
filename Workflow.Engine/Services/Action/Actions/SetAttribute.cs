using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OptimaJet.Workflow.Core.Model;
using OptimaJet.Workflow.Core.Runtime;
using System.Threading.Tasks;
using System.Threading;
using NCalc;
using Workflow.Engine.Services.Action.Utils;
using Workflow.Engine.Services.Action.Dto;

namespace Workflow.Engine.Services
{
    public partial class ActionService
    {
        private void SetAttribute(ProcessInstance process, WorkflowRuntime runtime, string parameters, JObject entity)
        {
            if (Util.TryDeserializeObject(parameters, out AttributeDtoInput attributeDtoInput) && attributeDtoInput.IsValid())
            {
                if (!String.IsNullOrEmpty(attributeDtoInput.Expression))
                {
                    var expression = Util.FindAutoMapExpression(attributeDtoInput.Expression, entity);

                    var e = new Expression(expression);

                    entity.Add(attributeDtoInput.Name, e.Evaluate().ToString());
                }
                else
                {
                    if (attributeDtoInput.Value.StartsWith("$"))
                    {
                        var attributeValue = Util.FindAutoMapExpression(attributeDtoInput.Value, entity);
                        entity.Add(attributeDtoInput.Name, attributeValue);
                    }
                    else
                    {
                        entity.Add(attributeDtoInput.Name, attributeDtoInput.Value);
                    }
                }
            }
            else
            {
                throw new Exception($"Can not Deserialize or are missed some input parameters");
            }
        }
    }
}
