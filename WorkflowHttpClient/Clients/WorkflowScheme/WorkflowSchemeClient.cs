using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WorkflowHttpClient.Dto;
using WorkflowHttpClient.Utils;

namespace WorkflowHttpClient.Clients
{
    public class WorkflowSchemeClient : BaseClient, IWorkflowSchemeClient
    {
        public WorkflowSchemeClient(string baseUrlPath, string apiKey) 
            : base(baseUrlPath, apiKey)
        {
            
        }

        public async Task<ResponseDto> GetAllSchemas(int maxResultCount, int skipCount, string sorting = null, string filter = null)
        {
            dynamic client = new Client(host: BaseUrlPath, requestHeaders: GlobalRequestHeaders, urlPath: UrlPath);
            var json = JsonConvert.SerializeObject(new { maxResultCount, skipCount, sorting, filter });
            return await client.Schemes.GetAll.get(queryParams: json);
        }

        public async Task<ResponseDto> GetSchema(string code)
        {
            dynamic client = new Client(host: BaseUrlPath, requestHeaders: GlobalRequestHeaders, urlPath: UrlPath);
            var json = JsonConvert.SerializeObject(new { code });
            return await client.Schemes.Get.get(queryParams: json);
        }


        public async Task<ResponseDto> DeleteSchema(string code)
        {
            dynamic client = new Client(host: BaseUrlPath, requestHeaders: GlobalRequestHeaders, urlPath: UrlPath);
            var json = JsonConvert.SerializeObject(new { code });
            return await client.Schemes.Delete.delete(queryParams: json);
        }

        public async Task<ResponseDto> CreateSchema(string code)
        {
            dynamic client = new Client(host: BaseUrlPath, requestHeaders: GlobalRequestHeaders, urlPath: UrlPath);
            var json = JsonConvert.SerializeObject(new { code });
            return await client.Schemes.Create.post(queryParams: json);
        }

        public async Task<ResponseDto> Designer(Dictionary<string, string> requestQueryParameters, Stream formStream = null)
        {
            dynamic client = new Client(host: BaseUrlPath, requestHeaders: GlobalRequestHeaders, urlPath: UrlPath);
            string content = null;

            if (formStream != null)
            {
                using (StreamReader sr = new StreamReader(formStream))
                {
                    content = sr.ReadToEnd();
                }

            }
            var json = JsonConvert.SerializeObject(new DesignerDto() { FormStream = content, RequestQueryParameters = requestQueryParameters });
            return await client.Schemes.Api.post(requestBody: json);
        }

        public async Task<ResponseDto> RunSchema(string code)
        {
            dynamic client = new Client(host: BaseUrlPath, requestHeaders: GlobalRequestHeaders, urlPath: UrlPath);
            var json = JsonConvert.SerializeObject(new { code });
            return await client.Schemes.Run.post(queryParams: json);
        }
    }
}