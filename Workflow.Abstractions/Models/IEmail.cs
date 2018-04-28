using System.Collections.Generic;

namespace Workflow.Abstractions.Models
{
    public interface IEmail
    {
        List<string> Addresses { get; }

        string Subject { get; }

        string Content { get; }
    }
}
