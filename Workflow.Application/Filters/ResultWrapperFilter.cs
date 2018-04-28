using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using Workflow.Application.Utils;

namespace Workflow.Application.Filters
{
    public class ResultWrapperFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var response = await next();

            if (response.Exception == null)
            {
                if (response.Result == null)
                {
                    response.Result = new ObjectResult(new ResultWrapper { IsSuccessful = true }) { StatusCode = StatusCodes.Status200OK };
                }
                else
                {
                    if (response.Result is ObjectResult result)
                    {
                        result.Value = new ResultWrapper { Result = result.Value, IsSuccessful = true };
                    }
                    else if(response.Result is EmptyResult emptyResult)
                    {
                        response.Result = new ObjectResult(new ResultWrapper { IsSuccessful = true }) { StatusCode = StatusCodes.Status200OK };
                    }
                }
            }
            else
            {
                if (response.Exception.GetType() == typeof(InvalidInputException))
                {
                    response.Result = new ObjectResult(new ResultWrapper { Message = response.Exception.Message }) { StatusCode = StatusCodes.Status200OK };
                }
                else
                {
                    response.Result = new ObjectResult(new ResultWrapper { HasErrors = true, ErrorMessage = response.Exception.Message }) { StatusCode = StatusCodes.Status500InternalServerError };
                }

                response.Exception = null;

                response.ExceptionHandled = true;
            }
        }
    }
}
