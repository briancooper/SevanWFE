using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WorkflowHttpClient.Clients.Dto;
using WorkflowHttpClient.Dto;

namespace WorkflowHttpClient.Clients
{
    public class TriggerClient : BaseClient, ITriggerClient
    {
        public TriggerClient(string baseUrlPath, string apiKey) : base(baseUrlPath, apiKey)
        {
        }

        public async Task<ResponseDto> Fire(string name, object parameters)
        {
            dynamic client = new Client(host: BaseUrlPath, requestHeaders: GlobalRequestHeaders, urlPath: UrlPath);
            var jsonBodyParam = JsonConvert.SerializeObject(parameters);
            var jsonQueryParam = JsonConvert.SerializeObject(new { name });
            return await client.Triggers.Fire.post(queryParams: jsonQueryParam, requestBody: jsonBodyParam);
        }
    }
}
