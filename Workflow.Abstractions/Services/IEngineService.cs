using System;
using System.Collections.Specialized;
using System.IO;
using System.Threading.Tasks;

namespace Workflow.Abstractions.Services
{
    public interface IEngineService
    {
        string Designer(NameValueCollection parameters, Stream stream);

        Task RunSchemeAsync(string code, Guid id);
    }
}
