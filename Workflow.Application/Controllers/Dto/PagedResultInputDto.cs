namespace Workflow.Application.Controllers.Dto
{
    public class PagedResultInputDto : IPagedResultInput
    {
        public int MaxResultCount { get; set; }

        public int SkipCount { get; set; }
    }
}
