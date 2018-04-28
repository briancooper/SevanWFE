using Workflow.Abstractions.Models;

namespace Workflow.Engine.Services.Action.Dto
{
    public class ReplaceDtoInput : IBindField
    {
        public string From { get; set; }

        public string To { get; set; }
    }
}
