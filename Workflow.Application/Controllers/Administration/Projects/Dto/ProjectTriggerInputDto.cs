using System;

namespace Workflow.Application.Controllers.Administration.Projects.Dto
{
    public class ProjectTriggerInputDto
    {
        public Guid ProjectId { get; set; }

        public Guid TriggerId { get; set; }
    }
}
