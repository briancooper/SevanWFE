using System;
using System.ComponentModel.DataAnnotations;

namespace Workflow.Core.Models.Triggers
{
    public class TriggerAttribute
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public Guid ProjectTriggerId { get; set; }
    }
}
