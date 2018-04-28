using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.Abstractions.Models;

namespace Workflow.Abstractions.Services
{
    public interface IConverterService
    {
        Task<string> ConvertToPdfAsync(string content);
        string FillPdfForm<T>(string filePath, List<T> fields) where T : IBindField;
        IEnumerable<IPdfFormFieldInfo> GetPdfFormFieldsList(string filePath);
    }
}
