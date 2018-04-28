using System;

namespace WorkflowHttpClient.Clients.Dto
{
    public class FormTemplateUpdateDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
