using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using OptimaJet.Workflow.Core.Model;
using OptimaJet.Workflow.Core.Runtime;
using System.Threading.Tasks;
using System.Threading;
using Workflow.Engine.Services.Action.Dto;
using Workflow.Engine.Services.Action.Utils;

namespace Workflow.Engine.Services
{
    public partial class ActionService
    {
        private bool IsUserCertified(ProcessInstance process, WorkflowRuntime runtime, string parameters, JObject entity)
        {
            if (Util.TryDeserializeObject(parameters, out ContentDtoInput contentDtoInput) && contentDtoInput.IsValid())
            {
                var source = Util.FindAutoMapExpression(contentDtoInput.Source, entity);
                var examInfo = Util.GetParameter(parameters, "ExamInfo", new UserCertifyDtoInput());

                var isCertified = double.Parse(source) >= examInfo.ExamPassPercentage;

                Util.AddOrUpdateAttribute(entity, contentDtoInput.Result, isCertified);

                return isCertified;
            }
            else
            {
                throw new Exception($"Can not Deserialize or are missed some input parameters");
            }
        }
    }
}
