using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Workflow.Core.Interfaces;

namespace Workflow.Core.Models.Triggers
{
    public class ProjectTrigger: ISoftDelete, IProjectRelated
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid TriggerId { get; set; }

        [ForeignKey("TriggerId")]
        public Trigger Trigger { get; set; }

        [Required]
        public Guid ProjectId { get; set; }

        public ICollection<TriggerAttribute> Attributes { get; set; }
    }
}
