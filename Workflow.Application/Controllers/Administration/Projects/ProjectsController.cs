using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Workflow.Abstractions.Database;
using Workflow.Abstractions.Services;
using Workflow.Application.Controllers.Dto;
using Workflow.Application.Extensions;
using Workflow.Core.Models.Projects;
using Workflow.Application.Controllers.Administration.Projects.Dto;
using Workflow.Core.Models.Triggers;
using Workflow.Application.Utils;

namespace Workflow.Application.Controllers.Administration
{
    public class ProjectsController : AdministrationBaseController
    {
        private readonly IRepository<Project> _projectsRepository;

        private readonly ISecurityService _securityService;

        private readonly IRepository<Trigger> _triggerRepository;

        private readonly IRepository<ProjectTrigger> _projectTriggerRepository;


        public ProjectsController(IRepository<Project> projectsRepository, IRepository<Trigger> triggerRepository, IRepository<ProjectTrigger> projectTriggerRepository, ISecurityService securityService)
        {
            _projectsRepository = projectsRepository;

            _triggerRepository = triggerRepository;

            _projectTriggerRepository = projectTriggerRepository;

            _securityService = securityService;
        }

        [HttpGet]
        public async Task<PagedResultDto<ProjectDto>> GetAll(FullResultInputDto input)
        {
            var projects = _projectsRepository.Queryable();

            return await projects.ToPagedResult<Project, ProjectDto>(input, p => p.Name.Contains(input.Filter));
        }

        [HttpPost]
        public async Task Create(ProjectCreateInputDto input)
        {
            if (string.IsNullOrWhiteSpace(input.Name))
            {
                throw new InvalidInputException(@"The name of the project is empty.");
            }

            var project = await _projectsRepository.FirstOrDefaultAsync(p => p.Name == input.Name);

            if (project != null)
            {
                throw new InvalidInputException(@"A project with this name already exists.");
            }

            var accessKey = _securityService.GenerateProjectAccessKey();

            do
            {
                project = await _projectsRepository.FirstOrDefaultAsync(p => p.AccessKey == accessKey);
            }
            while (project != null);

            project = new Project
            {
                Name = input.Name,

                AccessKey = accessKey
            };

            await _projectsRepository.InsertAsync(project);
        }

        [HttpPut]
        public async Task Update(ProjectDto input)
        {
            if (string.IsNullOrWhiteSpace(input.Name))
            {
                throw new InvalidInputException(@"The name of the prject is empty.");
            }

            var project = await _projectsRepository.FirstOrDefaultAsync(p => p.Name == input.Name);

            if (project != null)
            {
                throw new InvalidInputException(@"A project with this name already exists.");
            }

            project = await _projectsRepository.FirstOrDefaultAsync(p => p.Id == input.Id);

            if (project == null)
            {
                throw new InvalidInputException(@"Project was not found.");
            }

            project.Name = input.Name;

            await _projectsRepository.UpdateAsync(project);
        }

        [HttpDelete]
        public async Task Delete(Guid id)
        {
            var project = await _projectsRepository.FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
            {
                throw new InvalidInputException(@"Project was not found.");
            }

            await _projectsRepository.RemoveAsync(project);
        }

        [HttpPut]
        public async Task AssignTrigger(ProjectTriggerInputDto input)
        {
            var project = await _projectsRepository.FirstOrDefaultAsync(p => p.Id == input.ProjectId);

            if (project == null)
            {
                throw new InvalidInputException(@"Project was not found.");
            }

            var trigger = await _triggerRepository.FirstOrDefaultAsync(p => p.Id == input.TriggerId);

            if (trigger == null)
            {
                throw new InvalidInputException(@"Trigger was not found.");
            }

            var projectTrigger = await _projectTriggerRepository.FirstOrDefaultAsync(p => p.TriggerId == trigger.Id);

            if (projectTrigger != null)
            {
                throw new InvalidInputException(@"The trigger has already been assigned.");
            }

            projectTrigger = new ProjectTrigger
            {
                TriggerId = trigger.Id,

                ProjectId = project.Id
            };

            await _projectTriggerRepository.InsertAsync(projectTrigger);
        }

        [HttpPut]
        public async Task UnassignTrigger(ProjectTriggerInputDto input)
        {
            var project = await _projectsRepository.FirstOrDefaultAsync(p => p.Id == input.ProjectId);

            if (project == null)
            {
                throw new InvalidInputException(@"Project was not found.");
            }

            var trigger = await _triggerRepository.FirstOrDefaultAsync(p => p.Id == input.TriggerId);

            if (trigger == null)
            {
                throw new InvalidInputException(@"Trigger was not found.");
            }

            var projectTrigger = await _projectTriggerRepository.FirstOrDefaultAsync(p => p.TriggerId == trigger.Id);

            if (projectTrigger == null)
            {
                throw new InvalidInputException(@"The trigger has already been unassigned.");
            }

            await _projectTriggerRepository.RemoveAsync(projectTrigger);
        }
    }
}
