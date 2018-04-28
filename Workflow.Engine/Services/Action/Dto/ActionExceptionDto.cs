using System.Collections.Generic;

namespace Workflow.Engine.Services.Action.Dto
{
    public class ActivityExceptionDto
    {
        public string ActivityName { get; set; }
        public List<ActionExceptionDto> ActionExceptionList { get; set; }
    }

    public class ActionExceptionDto 
    {
        public string ActionName { get; set; }

        public string ExceptionMessage { get; set; }

        public string ExceptionStackTrace { get; set; }
    }
}
