using System;
using System.Collections.Generic;
using System.Text;

namespace Workflow.Abstractions.Models
{
    public interface IUser
    {
        string Username { get; set; }

        string Password { get; set; }

        string Role { get; set; }
    }
}
