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
        private async Task AmazonS3UploadFile(ProcessInstance process, WorkflowRuntime runtime, string parameters, CancellationToken cancellationToken, JObject entity)
        {
            if (Util.TryDeserializeObject(parameters, out ContentDtoInput contentDtoInput) && contentDtoInput.IsValid())
            {
                var filePath = Util.FindAutoMapExpression(contentDtoInput.Source, entity);

                var url = await _storageService.AmazonS3UploadFileAsync(filePath);

                if (string.IsNullOrWhiteSpace(url))
                {
                    throw new Exception($"Amazon storage url is empty");
                }

                Util.AddOrUpdateAttribute(entity, contentDtoInput.Result, url);
            }
            else
            {
                throw new Exception($"Can not Deserialize or are missed some input parameters");
            }
        }
    }
}
