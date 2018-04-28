namespace Workflow.Application.Controllers.Dto
{
    public class PagedAndFilteredResultInputDto : IPagedResultInput, IFilteredResultInput
    {
        public int MaxResultCount { get; set; }

        public int SkipCount { get; set; }

        public string Filter { get; set; }
    }
}
