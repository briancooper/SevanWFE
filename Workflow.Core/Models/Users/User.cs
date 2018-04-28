using System;
using System.Collections.Generic;
using System.Text;
using Workflow.Abstractions.Models;

namespace Workflow.Core.Models.Users
{
    public class User : IUser
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }
    }
}
