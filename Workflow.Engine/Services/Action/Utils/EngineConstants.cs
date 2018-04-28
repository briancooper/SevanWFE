using System;
using System.Collections.Generic;
using System.Text;

namespace Workflow.Engine.Services.Action.Utils
{
    public class EngineConstants
    {
        public const string ExecuteActionException = "ExecuteActionException";
        public const string SetEntityAttributeEndpoint = "/api/services/app/attributesService/CreateAndSetAttribute";
        public const string RemoveEntityAttributeEndpoint = "/api/services/app/attributesService/RemoveAttributes";
        public const string SetUserRolesEndpoint = "/api/Workflow/GrantRoleToUser";
        public const string RemoveUserRolesEndpoint = "/api/Workflow/RemoveRoleFromUser";
    }
}
