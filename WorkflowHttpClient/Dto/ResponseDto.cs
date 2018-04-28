using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WorkflowHttpClient.Dto
{
    public class ResponseDto
    {
        public int ResponseCode;
        public string ResponseContent;
        public Dictionary<string,IEnumerable<string>> ResponseHeaders;

        public ResponseDto(int responseCode, string responseContent, Dictionary<string, IEnumerable<string>> responseHeaders)
        {
            ResponseCode = responseCode;
            ResponseContent = responseContent;
            ResponseHeaders = responseHeaders;
        }
    }
}