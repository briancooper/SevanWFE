using System;
using System.ComponentModel.DataAnnotations;

namespace Workflow.Core.Models
{
    public class WorkflowProcessScheme
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Scheme { get; set; }

        [Required]
        public string DefiningParameters { get; set; }

        [Required]
        [MaxLength(1024)]
        public string DefiningParametersHash { get; set; }

        [Required]
        public string SchemeCode { get; set; }

        [Required]
        public bool IsObsolete { get; set; }

        public string RootSchemeCode { get; set; }

        public Guid? RootSchemeId { get; set; }

        public string AllowedActivities { get; set; }

        public string StartingTransition { get; set; }
    }
}
