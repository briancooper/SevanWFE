namespace Workflow.Application.Controllers.Dto
{
    public class SortedAndFilteredResultInputDto : ISortedResultInput, IFilteredResultInput
    {
        public string Sorting { get; set; }

        public string Filter { get; set; }
    }
}
