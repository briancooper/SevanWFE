using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Workflow.Abstractions.Database;
using Workflow.Abstractions.Services;
using Workflow.Application.Controllers.Dto;
using Workflow.Application.Controllers.Templates.Dto;
using Workflow.Application.Extensions;
using Workflow.Application.Utils;
using Workflow.Core.Models.Templates;
using Workflow.Core.Security;

namespace Workflow.Application.Controllers
{
    [Authorize(Roles = Roles.Project)]
    public class TemplatesController : BaseController
    {
        private readonly IRepository<Template> _templatesRepository;

        private readonly IRepository<FormTemplate> _formTemplatesRepository;

        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly IConverterService _converterService;
        

        public TemplatesController(IRepository<Template> templatesRepository, IRepository<FormTemplate> formTemplatesRepository, IHostingEnvironment hostingEnvironment, IConverterService converterService)
        {
            _templatesRepository = templatesRepository;

            _formTemplatesRepository = formTemplatesRepository;

            _hostingEnvironment = hostingEnvironment;

            _converterService = converterService;
        }

        [HttpGet]
        public async Task<PagedResultDto<TemplateShortResultDto>> GetAll(FullResultInputDto input)
        {
            var templates = _templatesRepository.Queryable();

            return await templates.ToPagedResult<Template, TemplateShortResultDto>(input, t => t.Name.Contains(input.Filter));
        }

        [HttpGet]
        public async Task<HtmlTemplateDto> Get(Guid id)
        {
            var template = await _templatesRepository.FirstOrDefaultAsync(x => x.Id == id);

            if (template == null)
            {
                throw new InvalidInputException(@"Template was not found.");
            }

            return Mapper.Map<HtmlTemplateDto>(template);
        }

        [HttpPost]
        public async Task<TemplateShortResultDto> Create([FromBody]HtmlTemplateCreateInputDto input)
        {
            if (!input.IsValid())
            {
                throw new ArgumentException(@"Template is invalid.");
            }

            var template = await _templatesRepository.FirstOrDefaultAsync(p => p.Name == input.Name);

            if (template != null)
            {
                throw new InvalidInputException(@"A template with this name already exists.");
            }

            template = Mapper.Map<Template>(input);

            var result = await _templatesRepository.InsertAsync(template);

            return Mapper.Map<TemplateShortResultDto>(result);
        }

        [HttpPut]
        public async Task Update([FromBody]HtmlTemplateDto input)
        {
            if (!input.IsValid())
            {
                throw new ArgumentException(@"Template is invalid.");
            }

            var template = await _templatesRepository.FirstOrDefaultAsync(x => x.Id == input.Id);

            if (template == null)
            {
                throw new InvalidInputException(@"Template was not found.");
            }

            template = Mapper.Map(input, template);

            await _templatesRepository.UpdateAsync(template);
        }

        [HttpDelete]
        public async Task Delete(Guid id)
        {
            var template = await _templatesRepository.FirstOrDefaultAsync(x => x.Id == id);

            if (template == null)
            {
                throw new InvalidInputException(@"Template was not found.");
            }

            await _templatesRepository.RemoveAsync(template);
        }

        [HttpGet]
        public async Task<PagedResultDto<TemplateShortResultDto>> GetAllForms(FullResultInputDto input)
        {
            var formTemplates = _formTemplatesRepository.Queryable();

            return await formTemplates.ToPagedResult<FormTemplate, TemplateShortResultDto>(input, ft => ft.Name.Contains(input.Filter));
        }

        [HttpGet]
        public async Task<FormTemplateDto> GetForm(Guid id)
        {
            var formTemplate = await _formTemplatesRepository.FirstOrDefaultAsync(x => x.Id == id);

            if (formTemplate == null)
            {
                throw new InvalidInputException(@"Form template was not found.");
            }

            return Mapper.Map<FormTemplateDto>(formTemplate);
        }

        [HttpPost]
        public async Task CreateForm([FromBody]FormTemplateCreateInputDto input)
        {
            if (!input.IsValid())
            {
                throw new ArgumentException(@"Form template is invalid.");
            }

            var formTemplate = await _formTemplatesRepository.FirstOrDefaultAsync(p => p.Name == input.Name);

            if (formTemplate != null)
            {
                throw new InvalidInputException(@"A form template with this name already exists.");
            }

            var folderPath = $"{_hostingEnvironment.ContentRootPath}\\FileTemplates\\";

            Directory.CreateDirectory(folderPath);

            var filePath = CreateUniqueFilePath(input.Name);

            formTemplate = Mapper.Map<FormTemplate>(input);

            System.IO.File.WriteAllBytes(filePath, Convert.FromBase64String(input.FileBytes));

            formTemplate.FilePath = filePath;

            var fieldsJson = JsonConvert.SerializeObject(_converterService.GetPdfFormFieldsList(filePath));

            formTemplate.PdfFormFields = fieldsJson;

            await _formTemplatesRepository.InsertAsync(formTemplate);
        }

        [HttpPut]
        public async Task UpdateForm([FromBody]FormTemplateDto input)
        {
            if (!input.IsValid())
            {
                throw new ArgumentException(@"Form template is invalid.");
            }

            var formTemplate = await _formTemplatesRepository.FirstOrDefaultAsync(x => x.Id == input.Id);

            if (formTemplate == null)
            {
                throw new InvalidInputException(@"Form template was not found.");
            }

            formTemplate = Mapper.Map(input, formTemplate);

            await _formTemplatesRepository.UpdateAsync(formTemplate);
        }

        [HttpDelete]
        public async Task DeleteForm(Guid id)
        {
            var formTemplate = await _formTemplatesRepository.FirstOrDefaultAsync(x => x.Id == id);

            if (formTemplate == null)
            {
                throw new InvalidInputException(@"Form template was not found.");
            }

            System.IO.File.Delete(formTemplate.FilePath);

            await _formTemplatesRepository.RemoveAsync(formTemplate);
        }

        [HttpGet]
        public async Task<PdfFormFieldsDto> GetPdfFormFields(Guid id)
        {
            var entity = await _formTemplatesRepository.FirstOrDefaultAsync(x => x.Id == id);

            if (string.IsNullOrEmpty(entity.PdfFormFields))
            {
                var fieldsJson = JsonConvert.SerializeObject(_converterService.GetPdfFormFieldsList(entity.FilePath));
                entity.PdfFormFields = fieldsJson;
                await _formTemplatesRepository.UpdateAsync(entity);
            }

            var fieldInfoes = JsonConvert.DeserializeObject<List<PdfFormFieldInfo>>(entity.PdfFormFields);

            var result = new PdfFormFieldsDto(id, fieldInfoes);

            return result;
        }

        private string CreateUniqueFilePath(string fileName)
        {
            if (!fileName.EndsWith(".pdf"))
            {
                fileName += ".pdf";
            }
            var filePath = $"{_hostingEnvironment.ContentRootPath}/FileTemplates/{fileName}";
            int iteration = 0;
            while (System.IO.File.Exists(filePath))
            {
                filePath = $"{_hostingEnvironment.ContentRootPath}/FileTemplates/({iteration++}){fileName}";
            }

            return filePath;
        }
    }
}
