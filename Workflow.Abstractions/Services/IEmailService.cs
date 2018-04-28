using System.Threading.Tasks;
using Workflow.Abstractions.Models;

namespace Workflow.Abstractions.Services
{
    public interface IEmailService
    {
        Task SendAsync(IEmail email);
    }
}
