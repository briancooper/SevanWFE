using System;
using WorkflowHttpClient.Clients.Dto;

namespace WorkflowHttpClient.Clients.Dto
{
    public class HtmlTemplateEditInputDto : HtmlTemplateInputDto
    {
        public Guid Id { get; set; }
    }
}
