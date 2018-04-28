using System;
using System.Threading.Tasks;
using WorkflowHttpClient.Clients.Dto;
using WorkflowHttpClient.Dto;

namespace WorkflowHttpClient.Clients
{
    public interface ITemplateClient
    {
        Task<ResponseDto> GetAll(int skip, int maxCount, string nameFilter = null, string sorting = null);
        Task<ResponseDto> Get(Guid id);
        Task<ResponseDto> Create(HtmlTemplateInputDto input);
        Task<ResponseDto> Update(HtmlTemplateEditInputDto input);
        Task<ResponseDto> Delete(Guid id);

        Task<ResponseDto> GetAllForms(int skip, int maxCount, string nameFilter = null, string sorting = null);
        Task<ResponseDto> GetForm(Guid id);
        Task<ResponseDto> CreateForm(FormTemplateCreateDto input);
        Task<ResponseDto> UpdateForm(FormTemplateUpdateDto input);
        Task<ResponseDto> DeleteForm(Guid id);
        Task<ResponseDto> GetPdfFormFields(Guid id);
    }
}
