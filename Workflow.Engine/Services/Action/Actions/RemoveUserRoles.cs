using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OptimaJet.Workflow.Core.Model;
using OptimaJet.Workflow.Core.Runtime;
using Workflow.Engine.Services.Action.Dto;
using Workflow.Engine.Services.Action.Utils;

namespace Workflow.Engine.Services
{
    public partial class ActionService
    {
        private void RemoveUserRoles(ProcessInstance process, WorkflowRuntime runtime, string parameters, JObject entity)
        {
            var source = Util.FindAutoMapExpression(parameters, entity);
            if (Util.TryDeserializeObject(source, out ApiRequestDtoInput apiRequestDtoInput))
            {
                apiRequestDtoInput.UrlAddress = apiRequestDtoInput.UrlAddress + EngineConstants.RemoveUserRolesEndpoint;
                var responce = Helpers.ApiRequest(apiRequestDtoInput);
                var jtokenResult = JsonConvert.DeserializeObject<JToken>(responce);
                entity.Add(apiRequestDtoInput.AttributeSuccessName, jtokenResult);
            }
            else
            {
                throw new Exception($"Can not Deserialize or are missed some input parameters");
            }
        }
    }
}
