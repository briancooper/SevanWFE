using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WorkflowHttpClient.Dto;

namespace WorkflowHttpClient.Clients
{
    public interface IWorkflowSchemeClient
    {
        Task<ResponseDto> GetAllSchemas(int maxResultCount, int skipCount, string sorting = null, string filter = null);
        Task<ResponseDto> GetSchema(string code);
        Task<ResponseDto> DeleteSchema(string code);
        Task<ResponseDto> Designer(Dictionary<string, string> requestQueryParameters, Stream formStream = null);
        Task<ResponseDto> RunSchema(string code);
    }
}