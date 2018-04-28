using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WorkflowHttpClient.Dto;
using WorkflowHttpClient.Utils;

namespace WorkflowHttpClient.Clients
{
    public class BaseClient
    {
        public string BaseUrlPath { get; set; }
        public string UrlPath { get; set; }
        public string ApiKey { get; set; }
        public string ApiToken { get; set; }
        public Dictionary<string, string> GlobalRequestHeaders { get; set; }

        public BaseClient(string baseUrlPath, string apiKey)
        {
            BaseUrlPath = baseUrlPath;
            UrlPath = WorkflowClientConstants.ApiRelativePath;
            ApiKey = apiKey;
            ApiToken = GetApiToken(apiKey);
            AddAuthorization();
        }

        private string GetApiToken(string apiKey)
        {
            var result = GetApiTokenAsync(apiKey).Result;
            var token = JsonConvert.DeserializeAnonymousType(result.ResponseContent, new { Result = new { Token = "" } });
            return token.Result.Token;
        }

        private async Task<ResponseDto> GetApiTokenAsync(string accessKey)
        {
            dynamic client = new Client(host: BaseUrlPath, requestHeaders: GlobalRequestHeaders, urlPath: UrlPath);
            var json = JsonConvert.SerializeObject(new { accessKey });
            return await client.Authorization.GetProjectToken.get(queryParams: json);
        }

        private void AddAuthorization()
        {
            GlobalRequestHeaders = new Dictionary<string, string>
            {
                {"Authorization", "Bearer " + ApiToken},
                {"Content-Type", "application/json"}
            };
        }
    }
}
