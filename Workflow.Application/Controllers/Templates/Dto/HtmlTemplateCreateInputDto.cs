namespace Workflow.Application.Controllers.Templates.Dto
{
    public class HtmlTemplateCreateInputDto
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }


        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Title) && !string.IsNullOrWhiteSpace(Content);
        }
    }
}
