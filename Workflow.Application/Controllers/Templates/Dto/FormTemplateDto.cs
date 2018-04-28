namespace Workflow.Application.Controllers.Templates.Dto
{
    public class FormTemplateDto : TemplateShortResultDto
    {
        public string Description { get; set; }


        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Title);
        }
    }
}
