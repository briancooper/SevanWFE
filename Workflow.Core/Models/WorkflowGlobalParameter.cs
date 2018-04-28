using System;
using System.ComponentModel.DataAnnotations;

namespace Workflow.Core.Models
{
    public class WorkflowGlobalParameter
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Value { get; set; }
    }
}