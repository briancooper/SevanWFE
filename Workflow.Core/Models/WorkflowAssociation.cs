using System;
using System.ComponentModel.DataAnnotations;

namespace Workflow.Core.Models
{
    public class WorkflowAssociation
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime CreationTime { get; set; }

        [Required]
        public string SchemeCode { get; set; }

        [Required]
        public SchemeStatus Status { get; set; }

        public string Entity { get; set; }


        public WorkflowAssociation()
        {
            CreationTime = DateTime.UtcNow;

            Status = SchemeStatus.Pending;
        }

        public enum SchemeStatus : int
        {
            Pending = 0,
            Running = 1,
            Done = 2
        }
    }
}
