using System;

namespace Workflow.Application.Controllers.Administration.Projects.Dto
{
    public class ProjectDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string AccessKey { get; set; }
    }
}
