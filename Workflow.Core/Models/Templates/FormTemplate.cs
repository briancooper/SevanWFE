using System;
using System.ComponentModel.DataAnnotations;

namespace Workflow.Core.Models.Templates
{
    public class FormTemplate : TemplateBase
    {
        [Required]
        public string FilePath { get; set; }

        public string PdfFormFields { get; set; }
    }
}
