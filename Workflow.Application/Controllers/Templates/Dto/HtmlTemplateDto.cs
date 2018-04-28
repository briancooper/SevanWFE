namespace Workflow.Application.Controllers.Templates.Dto
{
    public class HtmlTemplateDto : TemplateShortResultDto
    {
        public string Description { get; set; }

        public string Content { get; set; }


        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Title) && !string.IsNullOrWhiteSpace(Content);
        }
    }
}
