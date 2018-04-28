using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Workflow.Abstractions.Models;
using Workflow.Core.Interfaces;

namespace Workflow.Core.Models.Triggers
{
    public class Trigger : ISoftDelete
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
