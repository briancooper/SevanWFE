using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workflow.Abstractions.Models;

namespace Workflow.Application.Utils
{
    public class Bag : IBag
    {
        public Guid CurrentProjectId { get; }

        public Bag(IHttpContextAccessor httpContextAccessor)
        {
            CurrentProjectId = Guid.Empty;

            var httpContext = httpContextAccessor.HttpContext;

            if (httpContext == null || httpContext.User == null || httpContext.User.Claims == null)
            {
                return;
            }

            var claim = httpContext.User.Claims.FirstOrDefault(c => c.Type == "Id");

            if (claim == null)
            {
                return;
            }

            if (!Guid.TryParse(claim.Value, out Guid result))
            {
                return;
            }

            CurrentProjectId = result;
        }
    }
}
