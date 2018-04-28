using System.Collections.Generic;

namespace WorkflowHttpClient.Dto
{
    public class DesignerDto
    {
        public string FormStream { get; set; }
        public Dictionary<string, string> RequestQueryParameters { get; set; }
    }
}