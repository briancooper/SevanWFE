namespace Workflow.Application.Controllers.Dto
{
    public class PagedAndSortedResultInputDto : IPagedResultInput, ISortedResultInput
    {
        public int MaxResultCount { get; set; }

        public int SkipCount { get; set; }

        public string Sorting { get; set; }
    }
}
