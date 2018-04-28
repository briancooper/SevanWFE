using System;
using System.ComponentModel.DataAnnotations;
using Workflow.Abstractions.Models;
using Workflow.Core.Interfaces;

namespace Workflow.Core.Models.Projects
{
    public class Project : IProject, ISoftDelete
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string AccessKey { get; set; }
    }
}
