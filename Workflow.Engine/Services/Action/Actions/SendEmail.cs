using System;
using System.Linq;
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
        private async Task SendEmail(ProcessInstance process, WorkflowRuntime runtime, string parameters, CancellationToken token, JObject entity)
        {
            if (Util.TryDeserializeObject(parameters, out EmailDtoInput emailDtoInput) && emailDtoInput.IsValid())
            {
                emailDtoInput.Content = Util.FindAutoMapExpression(emailDtoInput.Content, entity);
                emailDtoInput.Addresses = emailDtoInput.Addresses.Select(ad =>
                      {
                          var address = Util.FindAutoMapExpression(ad, entity) == string.Empty ? ad : Util.FindAutoMapExpression(ad, entity);
                          return address.ToString();

                      }).ToList();

                emailDtoInput.Content = Util.FindAutoMapExpression(emailDtoInput.Content, entity);

                await _emailService.SendAsync(emailDtoInput);
            }
            else
            {
                throw new Exception($"Can not Deserialize or are missed some input parameters");
            }
        }
    }
}
