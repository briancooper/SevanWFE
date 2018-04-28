using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WorkflowHttpClient.Clients.Dto;
using WorkflowHttpClient.Dto;

namespace WorkflowHttpClient.Clients
{
    public interface ITriggerClient
    {
        Task<ResponseDto> Fire(string name, object parameters);
    }
}
