using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WorkflowHttpClient.Clients.Dto;
using WorkflowHttpClient.Dto;

namespace WorkflowHttpClient.Clients
{
    public class TemplateClient : BaseClient, ITemplateClient
    {
        public TemplateClient(string baseUrlPath, string apiKey) : base(baseUrlPath, apiKey)
        {
        }
        
        public async Task<ResponseDto> GetAll(int skipCount, int maxResultCount, string filter = null, string sorting = null)
        {
            dynamic client = new Client(host: BaseUrlPath, requestHeaders: GlobalRequestHeaders, urlPath: UrlPath);
            var json = JsonConvert.SerializeObject(new { maxResultCount, skipCount, sorting, filter });
            return await client.Templates.GetAll.get(queryParams: json);
        }

        public async Task<ResponseDto> Get(Guid id)
        {
            dynamic client = new Client(host: BaseUrlPath, requestHeaders: GlobalRequestHeaders, urlPath: UrlPath);
            var json = JsonConvert.SerializeObject(new {id});
            return await client.Templates.Get.get(queryParams: json);
        }

        public async Task<ResponseDto> Create(HtmlTemplateInputDto input)
        {
            dynamic client = new Client(host: BaseUrlPath, requestHeaders: GlobalRequestHeaders, urlPath: UrlPath);
            var json = JsonConvert.SerializeObject(input);
            return await client.Templates.Create.post(requestBody: json);
        }

        public async Task<ResponseDto> Update(HtmlTemplateEditInputDto input)
        {
            dynamic client = new Client(host: BaseUrlPath, requestHeaders: GlobalRequestHeaders, urlPath: UrlPath);
            var json = JsonConvert.SerializeObject(input);
            return await client.Templates.Update.put(requestBody: json);
        }

        public async Task<ResponseDto> Delete(Guid id)
        {
            dynamic client = new Client(host: BaseUrlPath, requestHeaders: GlobalRequestHeaders, urlPath: UrlPath);
            var json = JsonConvert.SerializeObject(new { id });
            return await client.Templates.Delete.delete(queryParams: json);
        }

        public async Task<ResponseDto> GetAllForms(int skipCount, int maxResultCount, string filter = null, string sorting = null)
        {
            dynamic client = new Client(host: BaseUrlPath, requestHeaders: GlobalRequestHeaders, urlPath: UrlPath);
            var json = JsonConvert.SerializeObject(new { maxResultCount, skipCount, sorting, filter });
            return await client.Templates.GetAllForms.get(queryParams: json);
        }

        public async Task<ResponseDto> GetForm(Guid id)
        {
            dynamic client = new Client(host: BaseUrlPath, requestHeaders: GlobalRequestHeaders, urlPath: UrlPath);
            var json = JsonConvert.SerializeObject(new { id });
            return await client.Templates.GetForm.get(queryParams: json);
        }

        public async Task<ResponseDto> CreateForm(FormTemplateCreateDto input)
        {
            dynamic client = new Client(host: BaseUrlPath, requestHeaders: GlobalRequestHeaders, urlPath: UrlPath);
            var json = JsonConvert.SerializeObject(input);
            return await client.Templates.CreateForm.post(requestBody: json);
        }

        public async Task<ResponseDto> UpdateForm(FormTemplateUpdateDto input)
        {
            dynamic client = new Client(host: BaseUrlPath, requestHeaders: GlobalRequestHeaders, urlPath: UrlPath);
            var json = JsonConvert.SerializeObject(input);
            return await client.Templates.UpdateForm.put(requestBody: json);
        }

        public async Task<ResponseDto> DeleteForm(Guid id)
        {
            dynamic client = new Client(host: BaseUrlPath, requestHeaders: GlobalRequestHeaders, urlPath: UrlPath);
            var json = JsonConvert.SerializeObject(new { id });
            return await client.Templates.DeleteForm.delete(queryParams: json);
        }

        public async Task<ResponseDto> GetPdfFormFields(Guid id)
        {
            dynamic client = new Client(host: BaseUrlPath, requestHeaders: GlobalRequestHeaders, urlPath: UrlPath);
            var json = JsonConvert.SerializeObject(new { id });
            return await client.Templates.GetPdfFormFields.get(queryParams: json);
        }
    }
}
