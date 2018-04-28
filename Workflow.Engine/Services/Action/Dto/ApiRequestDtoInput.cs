using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Workflow.Engine.Services.Action.Dto
{
    public class ApiRequestDtoInput
    {
        public string UrlAddress { get; set; }
        public JObject Content { get; set; }
        public bool IsQueryString { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public string RequestMethod { get; set; }
        public string AttributeSuccessName { get; set; }
        public string AttributeFailName { get; set; }
    }
}
