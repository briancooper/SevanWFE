using System.Threading.Tasks;
using Workflow.Abstractions.Models;

namespace Workflow.Abstractions.Services
{
    public interface ISecurityService
    {
        string GenerateProjectToken(IProject project);

        string GenerateUserToken(IUser user);

        string GenerateProjectAccessKey();
    }
}
