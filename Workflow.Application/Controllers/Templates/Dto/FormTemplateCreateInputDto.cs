namespace Workflow.Application.Controllers.Templates.Dto
{
    public class FormTemplateCreateInputDto
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string FileBytes { get; set; }


        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Title) && !string.IsNullOrWhiteSpace(FileBytes);
        }
    } 
}
