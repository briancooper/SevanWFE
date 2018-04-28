using System;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using OptimaJet.Workflow.Core.Model;
using OptimaJet.Workflow.Core.Runtime;
using Workflow.Engine.Services.Action.Utils;
using Workflow.Engine.Services.Action.Dto;

namespace Workflow.Engine.Services
{
    public partial class ActionService
    {
        private void FillTemplate(ProcessInstance process, WorkflowRuntime runtime, string parameters, JObject entity)
        {
            if (Util.TryDeserializeObject(parameters, out ContentDtoInput contentDtoInput) && contentDtoInput.IsValid())
            {
                var templateContent = Util.FindAutoMapExpression(contentDtoInput.Source, entity);

                if (string.IsNullOrWhiteSpace(templateContent))
                {
                    throw new Exception($"{contentDtoInput.Source} was not found");
                }

                var tmp = new StringBuilder(templateContent);

                tmp = Util.ManuallyFillTemplate(tmp, Util.GetParameter(parameters, "Replace", new List<ReplaceDtoInput>()), entity); //!!

                tmp = Util.AutoFillTemplate(tmp, entity); //!!

                var result = Util.AutoFillIteratorsTemplate(tmp, entity[Util.GetParameter(parameters, "TemplateIterator", "TemplateIterator")]); //!!

                Util.AddOrUpdateAttribute(entity, contentDtoInput.Result, result);
            }
            else
            {
                throw new Exception($"Can not Deserialize or are missed some input parameters");
            }
        }
    }
}
