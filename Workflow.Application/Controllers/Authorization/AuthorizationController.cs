using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Workflow.Abstractions.Database;
using Workflow.Abstractions.Services;
using Workflow.Application.Controllers.Authorization.Dto;
using Workflow.Core.Security;
using Workflow.Core.Models.Projects;
using Workflow.Core.Models.Users;
using Workflow.Application.Utils;

namespace Workflow.Application.Controllers
{
    public class AuthorizationController : BaseController
    {
        private readonly IRepository<Project> _projectsRepository;

        private readonly ISecurityService _securityService;


        public AuthorizationController(IRepository<Project> projectsRepository, ISecurityService securityService)
        {
            _projectsRepository = projectsRepository;

            _securityService = securityService;
        }

        [HttpGet]
        public async Task<TokenDto> GetProjectToken(string accessKey)
        {
            if (string.IsNullOrWhiteSpace(accessKey))
            {
                throw new InvalidInputException(@"Invalid access key.");
            }

            var project = await _projectsRepository.FirstOrDefaultAsync(p => p.AccessKey == accessKey);

            if (project == null)
            {
                throw new InvalidInputException(@"Project was not found.");
            }

            var token = _securityService.GenerateProjectToken(project);
          
            return new TokenDto { Token = token };
        }

        [HttpGet]
        public async Task<TokenDto> GetToken(string username, string password)
        {
            await Task.Delay(500);

            //if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            //{
            //    throw new InvalidInputException(@"Invalid credentials.");
            //}

            //var project = await _projectsRepository.FirstOrDefaultAsync(p => p.Name == projectName && p.AccessKey == accessKey);

            //if (project == null)
            //{
            //    throw new InvalidInputException(@"Project was not found.");
            //}

            var token = _securityService.GenerateUserToken(new User { Username = "Admin", Role = Roles.Administrator });

            return new TokenDto { Token = token };
        }
    }
}
