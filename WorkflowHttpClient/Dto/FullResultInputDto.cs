using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WorkflowHttpClient.Dto
{
    public class FullResultInputDto
    {
        public FullResultInputDto(int maxResultCount, int skipCount, string sorting, string filter)
        {
            MaxResultCount = maxResultCount;
            SkipCount = skipCount;
            Sorting = sorting;
            Filter = filter;
        }

        public int MaxResultCount { get; set; }

        public int SkipCount { get; set; }

        public string Sorting { get; set; }

        public string Filter { get; set; }
    }
}