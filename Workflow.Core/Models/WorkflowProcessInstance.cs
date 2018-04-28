using System;
using System.ComponentModel.DataAnnotations;

namespace Workflow.Core.Models
{
    public class WorkflowProcessInstance
    {
        [Key]
        public Guid Id { get; set; }

        public string StateName { get; set; }

        [Required]
        public string ActivityName { get; set; }

        public Guid? SchemeId { get; set; }

        public string PreviousState { get; set; }

        public string PreviousStateForDirect { get; set; }

        public string PreviousStateForReverse { get; set; }

        public string PreviousActivity { get; set; }

        public string PreviousActivityForDirect { get; set; }

        public string PreviousActivityForReverse { get; set; }

        public Guid? ParentProcessId { get; set; }

        [Required]
        public Guid RootProcessId { get; set; }

        [Required]
        public bool IsDeterminingParametersChanged { get; set; }
    }
}
