using Microsoft.AspNetCore.Mvc;

namespace Workflow.Application.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class BaseController : Controller
    {

    }
}
