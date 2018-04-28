using System;
using Newtonsoft.Json.Linq;
using OptimaJet.Workflow.Core.Model;
using OptimaJet.Workflow.Core.Runtime;
using System.Threading;
using System.Threading.Tasks;
using Workflow.Engine.Services.Action.Dto;
using Workflow.Engine.Services.Action.Utils;

namespace Workflow.Engine.Services
{
    public partial class ActionService
    {
        private async Task TemplateToPdf(ProcessInstance process, WorkflowRuntime runtime, string parameters, CancellationToken cancellationToken, JObject entity)
        {
            if (Util.TryDeserializeObject(parameters, out ContentDtoInput contentDtoInput) && contentDtoInput.IsValid())
            {
                var content = Util.FindAutoMapExpression(contentDtoInput.Source, entity);

                if (string.IsNullOrWhiteSpace(content))
                {
                    throw new Exception($"{contentDtoInput.Source} was not found");
                }

                var path = await _converterService.ConvertToPdfAsync(content);

                if (string.IsNullOrWhiteSpace(path))
                {
                    throw new Exception($"Converting to Pdf doesn't finished correctly"); ;
                }

                Util.AddOrUpdateAttribute(entity, contentDtoInput.Result, path);
            }
            else
            {
                throw new Exception($"Can not Deserialize or are missed some input parameters");
            }
        }
    }
}
