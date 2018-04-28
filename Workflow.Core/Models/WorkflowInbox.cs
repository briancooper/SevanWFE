using System;
using System.ComponentModel.DataAnnotations;

namespace Workflow.Core.Models
{
    public class WorkflowInbox
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ProcessId { get; set; }

        [Required]
        public Guid IdentityId { get; set; }
    }
}
