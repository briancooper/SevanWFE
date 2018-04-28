using System;
using System.Collections.Generic;
using System.Text;

namespace Workflow.Core.Security
{
    public static class Roles
    {
        public const string Administrator = "Administrator";

        public const string Project = "Project";

        public const string AdministratorAndProject = Administrator + "," + Project;
    }
}
