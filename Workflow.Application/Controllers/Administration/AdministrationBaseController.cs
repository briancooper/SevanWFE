using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Workflow.Core.Security;

namespace Workflow.Application.Controllers.Administration
{
    [Route("api/Administration/[controller]/[action]")]
    [Authorize(Roles = Roles.Administrator)]
    public class AdministrationBaseController : BaseController
    {

    }
}
