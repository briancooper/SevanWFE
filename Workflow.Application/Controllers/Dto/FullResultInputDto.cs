namespace Workflow.Application.Controllers.Dto
{
    public class FullResultInputDto : IFullResultInput, IPagedResultInput, ISortedResultInput, IFilteredResultInput
    {
        public int MaxResultCount { get; set; }

        public int SkipCount { get; set; }

        public string Sorting { get; set; }

        public string Filter { get; set; }
    }
}
