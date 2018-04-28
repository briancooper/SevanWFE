using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Workflow.Abstractions.Models;
using Workflow.Core.Interfaces;
using Workflow.Core.Models.Triggers;

namespace Workflow.Core.Models
{
    public class WorkflowScheme : ISoftDelete, IProjectRelated
    {
        [Key]
        [MaxLength(256)]
        public string Code { get; set; }

        [Required]
        public string Scheme { get; set; }

        public Guid? TriggerId { get; set; }

        [ForeignKey("TriggerId")]
        public Trigger Trigger { get; set; }
    }
}