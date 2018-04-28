using System;
using System.ComponentModel.DataAnnotations;

namespace Workflow.Core.Models
{
    public class WorkflowProcessInstancePersistence
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ProcessId { get; set; }

        [Required]
        public string ParameterName { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
