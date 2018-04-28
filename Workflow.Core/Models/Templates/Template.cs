using System;
using System.ComponentModel.DataAnnotations;

namespace Workflow.Core.Models.Templates
{
    public class Template : TemplateBase
    {
        [Required]
        public string Content { get; set; }
    }
}
