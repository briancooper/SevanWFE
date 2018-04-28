using System.Collections.Generic;
using System.Linq;
using Workflow.Abstractions.Models;

namespace Workflow.Engine.Services.Action.Dto
{
    public class EmailDtoInput : IEmail
    {
        public List<string> Addresses { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }


        public bool IsValid()
        {
            if (Addresses.Any(string.IsNullOrWhiteSpace) || string.IsNullOrWhiteSpace(Subject) || string.IsNullOrWhiteSpace(Content))
            {
                return false;
            }

            return true;
        }
    }
}
