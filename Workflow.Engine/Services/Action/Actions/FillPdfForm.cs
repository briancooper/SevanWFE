using System;
using Newtonsoft.Json.Linq;
using OptimaJet.Workflow.Core.Model;
using OptimaJet.Workflow.Core.Runtime;
using System.Collections.Generic;
using Workflow.Abstractions.Models;
using Workflow.Engine.Services.Action.Dto;
using Workflow.Engine.Services.Action.Utils;

namespace Workflow.Engine.Services
{
    public partial class ActionService
    {
        private void FillPdfForm(ProcessInstance process, WorkflowRuntime runtime, string parameters, JObject entity)
        {
            if (!Util.TryDeserializeObject(parameters, out ContentDtoInput contentDtoInput))
            {
                throw new Exception($"Can not Deserialize or are missed some input parameters");
            }

            var filePath = Util.FindAutoMapExpression(contentDtoInput.Source, entity);

            var fields = Util.GetParameter(parameters, "Fields", new List<ReplaceDtoInput>());

            if (string.IsNullOrEmpty(filePath) || fields == null)
            {
                throw new Exception($"Can not Deserialize or are missed some input parameters");
            }

            var finalFilePath = _converterService.FillPdfForm(filePath, fields);

            Util.AddOrUpdateAttribute(entity, contentDtoInput.Result, finalFilePath);
        }
    }
}
