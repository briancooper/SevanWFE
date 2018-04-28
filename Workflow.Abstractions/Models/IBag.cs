using System;

namespace Workflow.Abstractions.Models
{
    public interface IBag
    {
        Guid CurrentProjectId { get; }
    }
}
