using System;
using System.ComponentModel.DataAnnotations;

namespace Workflow.Core.Models
{
    public class WorkflowProcessTimer
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ProcessId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime NextExecutionDateTime { get; set; }

        [Required]
        public bool Ignore { get; set; }
    }
}
