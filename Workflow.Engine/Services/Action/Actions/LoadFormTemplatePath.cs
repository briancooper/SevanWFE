using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using OptimaJet.Workflow.Core.Model;
using OptimaJet.Workflow.Core.Runtime;
using Workflow.Engine.Services.Action.Dto;
using Workflow.Engine.Services.Action.Utils;

namespace Workflow.Engine.Services
{
    public partial class ActionService
    {
        private async Task LoadFormTemplatePath(ProcessInstance process, WorkflowRuntime runtime, string parameters,
            CancellationToken cancellationToken, JObject entity)
        {
            if (Util.TryDeserializeObject(parameters, out ContentDtoInput contentDtoInput) && contentDtoInput.IsValid())
            {
                var template = await _formTemplateRepository.FirstOrDefaultAsync(x => x.Name == contentDtoInput.Source);

                if (template == null)
                {
                    throw new Exception($"{contentDtoInput.Source} was not found");
                }

                Util.AddOrUpdateAttribute(entity, contentDtoInput.Result, template.FilePath);
            }
            else
            {
                throw new Exception($"Can not Deserialize or are missed some input parameters");
            }
        }
    }
}
