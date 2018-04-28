using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workflow.Abstractions.Database;
using Workflow.Abstractions.Services;
using Workflow.Application.Controllers.Dto;
using Workflow.Application.Controllers.Schemes.Dto;
using Workflow.Application.Extensions;
using Workflow.Application.Utils;
using Workflow.Core.Models;
using Workflow.Core.Security;
using static System.Net.WebRequestMethods;

namespace Workflow.Application.Controllers
{
    [Authorize(Roles = Roles.Project)]
    public class SchemesController : BaseController
    {
        private readonly IEngineService _workflowService;

        private readonly IRepository<WorkflowScheme> _workflowSchemesRepository;

        private readonly IRepository<WorkflowAssociation> _workflowAssociationsRepository;


        public SchemesController(IEngineService workflowService, IRepository<WorkflowScheme> workflowSchemesRepository, IRepository<WorkflowAssociation> workflowAssociationsRepository)
        {
            _workflowService = workflowService;

            _workflowSchemesRepository = workflowSchemesRepository;

            _workflowAssociationsRepository = workflowAssociationsRepository;
        }

        [HttpGet]
        public async Task<PagedResultDto<SchemeShortDto>> GetAll(FullResultInputDto input)
        {
            var schemes = _workflowSchemesRepository.Queryable();

            return await schemes.ToPagedResult<WorkflowScheme, SchemeShortDto>(input, p => p.Code.Contains(input.Filter));
        }

        [HttpGet]
        public async Task<SchemeDto> Get(string code)
        {
            var scheme = await _workflowSchemesRepository.FirstOrDefaultAsync(x => x.Code == code);

            if (scheme == null)
            {
                throw new InvalidInputException(@"Scheme was not found.");
            }

            return Mapper.Map<SchemeDto>(scheme);
        }

        [HttpPost]
        public async Task Create(string code)
        {
            var scheme = await _workflowSchemesRepository.FirstOrDefaultAsync(s=>s.Code == code);

            if (scheme != null)
            {
                throw new InvalidInputException(@"A scheme with this name already exists.");
            }

            scheme = new WorkflowScheme
            {
                Code = code,

                Scheme = string.Empty
            };

            await _workflowSchemesRepository.InsertAsync(scheme);
        }

        //WorkflowEngine related
        //[HttpPut]
        //public async Task Update(SchemeUpdateInputDto input)
        //{
        //    var scheme = await _workflowSchemesRepository.FirstOrDefaultAsync(ws => ws.Code == input.Code);

        //    if (scheme == null)
        //    {
        //        throw new InvalidInputException("The scheme not found.");
        //    }

        //    scheme.Code = input.NewCode;

        //    await _workflowSchemesRepository.UpdateAsync(scheme);
        //}

        [HttpDelete]
        public async Task Delete(string code)
        {
            var scheme = await _workflowSchemesRepository.FirstOrDefaultAsync(ws => ws.Code == code);

            if (scheme == null)
            {
                throw new InvalidInputException("The scheme not found.");
            }

            await _workflowSchemesRepository.RemoveAsync(scheme);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Api()
        {
            Stream stream = null;

            var allParameters = new NameValueCollection();

            foreach (var parameter in Request.Query)
            {
                allParameters.Add(parameter.Key, parameter.Value); //parameter.Value.First()
            }

            if (Request.Method.Equals(Http.Post, StringComparison.OrdinalIgnoreCase))
            {
                var keys = allParameters.AllKeys;
                foreach (var parameter in Request.Form)
                {
                    if (!keys.Contains(parameter.Key))
                    {
                        allParameters.Add(parameter.Key, parameter.Value); //parameter.Value.First()
                    }
                }

                if (Request.Form.Files != null && Request.Form.Files.Count > 0)
                {
                    stream = Request.Form.Files[0].OpenReadStream();
                }
            }

            if (allParameters["operation"].Equals("save", StringComparison.InvariantCultureIgnoreCase))
            {
                allParameters["schemecode"] = "super";
            }

            var result = _workflowService.Designer(allParameters, stream);

            if (allParameters["operation"].Equals("downloadscheme", StringComparison.InvariantCultureIgnoreCase))
            {
                return File(Encoding.UTF8.GetBytes(result), "text/xml", "scheme.xml");
            }

            return Content(result);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public IActionResult Api([FromBody]DesignerDto designerDto)
        {
            var allParameters = new NameValueCollection();
            var keys = designerDto.RequestQueryParameters.ToList();
            foreach (var parameter in keys)
            {
                allParameters.Add(parameter.Key, parameter.Value);
            }

            Stream stream = null;
            if (designerDto.FormStream != null)
            {
                var byteArray = Encoding.ASCII.GetBytes(designerDto.FormStream);
                stream = new MemoryStream(byteArray);
            }

            var result = _workflowService.Designer(allParameters, stream);

            if (allParameters["operation"].Equals("downloadscheme", StringComparison.InvariantCultureIgnoreCase))
            {
                return File(Encoding.UTF8.GetBytes(result), "text/xml", "scheme.xml");
            }

            return Content(result);
        }


        [HttpPost]
        public async Task<string> Run(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return "The name of the Workflow scheme is empty.";
            }

            var scheme = await _workflowSchemesRepository.FirstOrDefaultAsync(ws => ws.Code == code);

            if (scheme == null)
            {
                return "The name of the Workflow scheme was not found.";
            }

            var workflowAssociation = new WorkflowAssociation
            {
                Id = Guid.NewGuid(),

                SchemeCode = scheme.Code,

                Entity = "{}"
            };

            await _workflowAssociationsRepository.InsertAsync(workflowAssociation);

            await _workflowService.RunSchemeAsync(workflowAssociation.SchemeCode, workflowAssociation.Id);

            return "Your request has been added to the Workflow queue and will be processed soon.";
        }
    }
}
