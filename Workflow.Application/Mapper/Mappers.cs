using Workflow.Application.Controllers.Administration.Projects.Dto;
using Workflow.Application.Controllers.Administration.Triggers.Dto;
using Workflow.Application.Controllers.Schemes.Dto;
using Workflow.Application.Controllers.Triggers.Dto;
using Workflow.Application.Controllers.Templates.Dto;
using Workflow.Core.Models;
using Workflow.Core.Models.Projects;
using Workflow.Core.Models.Templates;
using Workflow.Core.Models.Triggers;

namespace Workflow.Application
{
    public static class Mappers
    {
        public static void Initialize()
        {
            AutoMapper.Mapper.Initialize(config =>
            {
                config.CreateMap<Project, ProjectDto>();
                config.CreateMap<WorkflowScheme, SchemeShortDto>();
                config.CreateMap<Template, TemplateShortResultDto>();
                config.CreateMap<HtmlTemplateCreateInputDto, Template>();
                config.CreateMap<Template, HtmlTemplateDto>();
                config.CreateMap<HtmlTemplateDto, Template>();
                config.CreateMap<FormTemplate, TemplateShortResultDto>();
                config.CreateMap<FormTemplate, FormTemplateDto>();
                config.CreateMap<FormTemplateDto, FormTemplate>();
                config.CreateMap<FormTemplateCreateInputDto, FormTemplate>();
                config.CreateMap<Trigger, TriggerDto>();
                config.CreateMap<Trigger, ProjectTriggerDto>();
            });
        }
    }
}
