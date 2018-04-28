using System;
using System.ComponentModel.DataAnnotations;
using Workflow.Abstractions.Models;
using Workflow.Core.Interfaces;

namespace Workflow.Core.Models.Templates
{
    public class TemplateBase : ISoftDelete, IProjectRelated
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime CreationTime { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }


        public TemplateBase()
        {
            CreationTime = DateTime.UtcNow;
        }
    }
}
