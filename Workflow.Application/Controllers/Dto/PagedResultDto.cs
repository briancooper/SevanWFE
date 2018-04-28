using System.Collections.Generic;

namespace Workflow.Application.Controllers.Dto
{
    public class PagedResultDto<T>
    {
        public IEnumerable<T> Items { get; }

        public int TotalCount { get; }


        public PagedResultDto(int totalCount, IEnumerable<T> items)
        {
            Items = items;

            TotalCount = totalCount;
        }
    }
}
