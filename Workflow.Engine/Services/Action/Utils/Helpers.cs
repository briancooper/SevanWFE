using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using OptimaJet.Workflow.Core.Model;
using OptimaJet.Workflow.Core.Runtime;
using Workflow.Engine.Services.Action.Dto;

namespace Workflow.Engine.Services.Action.Utils
{
    public static class Helpers
    {
        public static void ActionErrorHandling(string activityName, string actionName, Exception exception, JObject entity)
        {
            var error = entity.SelectToken(EngineConstants.ExecuteActionException);

            if (error != null)
            {
                var activity = error.Children().FirstOrDefault(o => o["ActivityName"] != null && o["ActivityName"].ToString() == activityName);
                if (activity != null)
                {
                    var actionExceptionList = (JArray)activity["ActionExceptionList"];
                    actionExceptionList.Add(JObject.FromObject(new ActionExceptionDto()
                    {
                        ActionName = actionName,
                        ExceptionMessage = exception.Message,
                        ExceptionStackTrace = exception.StackTrace
                    }
                    ));
                }
                else
                {
                    var errorList = (JArray)error;

                    errorList.Add(JObject.FromObject(new ActivityExceptionDto()
                    {
                        ActivityName = activityName,
                        ActionExceptionList = new List<ActionExceptionDto>() { new ActionExceptionDto(){
                                ActionName = actionName,
                                ExceptionMessage = exception.Message,
                                ExceptionStackTrace = exception.StackTrace
                            }
                        }
                    }));
                }
            }
            else
            {
                entity.Add(EngineConstants.ExecuteActionException, new JArray(JObject.FromObject(new ActivityExceptionDto()
                {
                    ActivityName = activityName,
                    ActionExceptionList = new List<ActionExceptionDto>() { new ActionExceptionDto(){
                            ActionName = actionName,
                            ExceptionMessage = exception.Message,
                            ExceptionStackTrace = exception.StackTrace
                        }
                    }
                })));
            }
        }
        public static async Task<string> ApiRequestAsync(ApiRequestDtoInput apiRequestDtoInput, CancellationToken cancellationToken)
        {
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage();
                if (apiRequestDtoInput.IsQueryString)
                {
                    request.RequestUri = new Uri(apiRequestDtoInput.UrlAddress + "?" +
                                                 JsonQueryStringConvert(apiRequestDtoInput.Content));
                }
                else
                {
                    request.RequestUri = new Uri(apiRequestDtoInput.UrlAddress);
                }

                request.Method = new HttpMethod(apiRequestDtoInput.RequestMethod);
                request.Content = new StringContent(JsonConvert.SerializeObject(apiRequestDtoInput.Content));
                foreach (var header in apiRequestDtoInput.Headers)
                {
                    switch (header.Key)
                    {
                        case "Content-Type":
                            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(header.Value);
                            break;
                        case "Authorization":
                            var match = Regex.Match(header.Value, "(bearer|basic)");
                            request.Headers.Authorization = match.Success ? new AuthenticationHeaderValue(match.Value, header.Value.Remove(match.Index, match.Length)) : new AuthenticationHeaderValue(header.Value);
                            break;
                        default:
                            break;
                    }
                }

                response = await client.SendAsync(request, cancellationToken);
            }
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            throw new Exception($"Status code{response.StatusCode}, Response reason phrase{response.ReasonPhrase}");
        }

        public static string ApiRequest(ApiRequestDtoInput apiRequestDtoInput)
        {
            HttpResponseMessage response;

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage();
                if (apiRequestDtoInput.IsQueryString)
                {
                    request.RequestUri = new Uri(apiRequestDtoInput.UrlAddress + "?" +
                                                 JsonQueryStringConvert(apiRequestDtoInput.Content));
                }
                else
                {
                    request.RequestUri = new Uri(apiRequestDtoInput.UrlAddress);
                }

                request.Method = new HttpMethod(apiRequestDtoInput.RequestMethod);
                request.Content = new StringContent(JsonConvert.SerializeObject(apiRequestDtoInput.Content));

                foreach (var header in apiRequestDtoInput.Headers)
                {
                    switch (header.Key)
                    {
                        case "Content-Type":
                            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(header.Value);
                            break;
                        case "Authorization":
                            var match = Regex.Match(header.Value, "(bearer|basic)");
                            request.Headers.Authorization = match.Success ? new AuthenticationHeaderValue(match.Value, header.Value.Remove(match.Index, match.Length)) : new AuthenticationHeaderValue(header.Value);
                            break;
                        default:
                            break;
                    }
                }
                response = client.SendAsync(request).Result;
            }
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsStringAsync().Result;
            }
            throw new Exception($"Status code{response.StatusCode}, Response reason phrase{response.ReasonPhrase}");
        }

        private static string JsonQueryStringConvert(JObject jObj)
        {
            return String.Join("&",
                jObj.Children().Cast<JProperty>()
                    .Select(jp => jp.Name + "=" + HttpUtility.UrlEncode(jp.Value.ToString())));
        }
    }
}
