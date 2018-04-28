using System.Collections.Generic;

namespace Workflow.Engine.Services.Action.Dto
{
    public class FillPdfFormInputDto
    {
        public Dictionary<string, string> Fields { get; set; }

        public string FilePath { get; set; }
    }
}
