using System;
using System.ComponentModel.DataAnnotations;

namespace Workflow.Core.Models
{
    public class WorkflowProcessInstanceStatus
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public byte Status { get; set; }

        [Required]
        public Guid Lock { get; set; }
    }
}
