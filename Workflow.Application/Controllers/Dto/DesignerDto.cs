using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;

namespace Workflow.Application.Controllers.Dto
{
    public class DesignerDto
    {
        public string FormStream { get; set; }
        public Dictionary<string, string> RequestQueryParameters { get; set; }
    }
}
