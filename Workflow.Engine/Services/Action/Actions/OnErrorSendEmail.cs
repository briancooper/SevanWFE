using System;
using System.Collections.Generic;
using System.Text;
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
        private async Task OnErrorSendEmail(ProcessInstance process, WorkflowRuntime runtime, string parameters, CancellationToken token, JObject entity)
        {
            var onErrorMessage = entity.SelectToken(EngineConstants.ExecuteActionException);

            if (onErrorMessage == null)
            {
                return;
            }

            if (Util.TryDeserializeObject(parameters, out EmailDtoInput emailDtoInput) && emailDtoInput.IsValid())
            {
                var errorList = onErrorMessage.ToObject<List<ActivityExceptionDto>>();
                var template = new StringBuilder();
                errorList.ForEach(e =>
                {
                    template.AppendLine(e.ActivityName);
                    template.AppendLine("<br/>");
                    e.ActionExceptionList.ForEach(m =>
                    {
                        template.Append("ActionName:  ");
                        template.AppendLine(m.ActionName);
                        template.Append("ExceptionMessage:  ");
                        template.AppendLine(m.ExceptionMessage);
                        template.Append("ExceptionStackTrace:  ");
                        template.AppendLine(m.ExceptionStackTrace);
                        template.AppendLine("<br/>");
                        template.AppendLine("<br/>");
                    });
                    template.AppendLine("<br/>");
                });
                emailDtoInput.Content = template.ToString();//Util.FindAutoMapExpression(emailDtoInput.Content, entity);

                await _emailService.SendAsync(emailDtoInput);
            }
            else
            {
                throw new Exception($"Can not Deserialize or are missed some input parameters");
            }
        }
    }
}
