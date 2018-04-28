using System;

namespace Workflow.Abstractions.Models
{
    public interface IProject
    {
        Guid Id { get; set; }

        string Name { get; set; }

        string AccessKey { get; set; }
    }
}
