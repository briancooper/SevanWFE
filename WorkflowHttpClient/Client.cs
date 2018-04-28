using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using WorkflowHttpClient.Dto;
using Newtonsoft.Json;

namespace WorkflowHttpClient
{
    public class Client : DynamicObject
    {
        private static HttpClient _httpClient = new HttpClient();
        public string Host;
        public Dictionary<string, string> RequestHeaders;
        public string Version;
        public string UrlPath;
        public string MediaType;
        public WebProxy WebProxy;
        public TimeSpan Timeout;

        public enum Methods
        {
            DELETE, GET, PATCH, POST, PUT
        }

        private int TimeoutDefault = 10;

        /// <summary>
        ///     REST API client.
        /// </summary>
        /// <param name="host">Base url (e.g. https://api.sendgrid.com)</param>
        /// <param name="requestHeaders">A dictionary of request headers</param>
        /// <param name="version">API version, override AddVersion to customize</param>
        /// <param name="urlPath">Path to endpoint (e.g. /path/to/endpoint)</param>
        /// <returns>Fluent interface to a REST API</returns>
        public Client(WebProxy webProxy, string host, Dictionary<string, string> requestHeaders = null, string version = null, string urlPath = null)
            : this(host, requestHeaders, version, urlPath)
        {
            WebProxy = webProxy;
        }


        /// <summary>
        ///     REST API client.
        /// </summary>
        /// <param name="host">Base url (e.g. https://api.sendgrid.com)</param>
        /// <param name="requestHeaders">A dictionary of request headers</param>
        /// <param name="version">API version, override AddVersion to customize</param>
        /// <param name="urlPath">Path to endpoint (e.g. /path/to/endpoint)</param>
        /// <param name="timeOut">Set an Timeout parameter for the HTTP Client</param>
        /// <returns>Fluent interface to a REST API</returns>
        public Client(string host, Dictionary<string, string> requestHeaders = null, string version = null, string urlPath = null, TimeSpan? timeOut = null)
        {
            Host = host;
            if (requestHeaders != null)
            {
                AddRequestHeader(requestHeaders);
            }
            Version = (version != null) ? version : null;
            UrlPath = (urlPath != null) ? urlPath : null;
            Timeout = (timeOut != null) ? (TimeSpan)timeOut : TimeSpan.FromSeconds(TimeoutDefault);
        }

        /// <summary>
        ///     Add requestHeaders to the API call
        /// </summary>
        /// <param name="requestHeaders">A dictionary of request headers</param>
        public void AddRequestHeader(Dictionary<string, string> requestHeaders)
        {
            RequestHeaders = (RequestHeaders != null)
                ? RequestHeaders.Union(requestHeaders).ToDictionary(pair => pair.Key, pair => pair.Value) : requestHeaders;
        }

        /// <summary>
        ///     Build the final URL
        /// </summary>
        /// <param name="queryParams">A string of JSON formatted query parameters (e.g {'param': 'param_value'})</param>
        /// <returns>Final URL</returns>
        private string BuildUrl(string queryParams = null)
        {
            string endpoint = null;

            if (Version != null)
            {
                endpoint = Host + "/" + Version + UrlPath;
            }
            else
            {
                endpoint = Host + UrlPath;
            }

            if (queryParams != null)
            {
                var ds_query_params = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(queryParams);
                var query = HttpUtility.ParseQueryString(string.Empty);
                foreach (var pair in ds_query_params)
                {
                    if (pair.Value != null)
                    {
                        query[pair.Key] = pair.Value.ToString(); 
                    }
                }
                string queryString = query.ToString();
                endpoint = endpoint + "?" + queryString;
            }

            return endpoint;
        }

        /// <summary>
        ///     Create a new Client object for method chaining
        /// </summary>
        /// <param name="name">Name of url segment to add to the URL</param>
        /// <returns>A new client object with "name" added to the URL</returns>
        private Client BuildClient(string name = null)
        {
            string endpoint;

            if (name != null)
            {
                endpoint = UrlPath + "/" + name;
            }
            else
            {
                endpoint = UrlPath;
            }

            UrlPath = null; // Reset the current object's state before we return a new one
            return new Client(Host, RequestHeaders, Version, endpoint, Timeout);

        }

        /// Factory method to return the right HttpClient settings.
        /// </summary>
        /// <returns>Instance of HttpClient</returns>
        private HttpClient BuildHttpClient()
        {
            // Add the WebProxy if set
            if (WebProxy != null)
            {
                var httpClientHandler = new HttpClientHandler()
                {
                    Proxy = WebProxy,
                    PreAuthenticate = true,
                    UseDefaultCredentials = false,
                };

                return new HttpClient(httpClientHandler);
            }

            return _httpClient;
        }

        /// <summary>

        /// <summary>
        ///     Add the authorization header, override to customize
        /// </summary>
        /// <param name="header">Authorization header</param>
        /// <returns>Authorization value to add to the header</returns>
        public virtual AuthenticationHeaderValue AddAuthorization(KeyValuePair<string, string> header)
        {
            string[] split = header.Value.Split(new char[0]);
            return new AuthenticationHeaderValue(split[0], split[1]);
        }

        /// <summary>
        ///     Add the version of the API, override to customize
        /// </summary>
        /// <param name="version">Version string to add to the URL</param>
        public virtual void AddVersion(string version)
        {
            Version = version;
        }

        /// <summary>
        ///     Deal with special cases and URL parameters
        /// </summary>
        /// <param name="name">Name of URL segment</param>
        /// <returns>A new client object with "name" added to the URL</returns>
        public Client _(string name)
        {
            return BuildClient(name);
        }

        /// <summary>
        ///     Reflection. We capture undefined variable access here
        /// </summary>
        /// <param name="binder">The calling object properties</param>
        /// <param name="result">The callback</param>
        /// <returns>The callback returns a new client object with "name" added to the URL</returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = BuildClient(binder.Name);
            return true;
        }

        /// <summary>
        ///     Reflection. We capture the final method call here
        /// </summary>
        /// <param name="binder">The calling object properties</param>
        /// <param name="args">The calling object's arguements</param>
        /// <param name="result">If "version", returns new client with version attached
        ///                      If "method", returns a Response object</param>
        /// <returns>The callback is described in "result"</returns>
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (binder.Name == "version")
            {
                AddVersion(args[0].ToString());
                result = BuildClient();
                return true;
            }

            if (Enum.IsDefined(typeof(Methods), binder.Name.ToUpper()))
            {
                CancellationToken cancellationToken = CancellationToken.None;
                string queryParams = null;
                string requestBody = null;
                int i = 0;

                foreach (object obj in args)
                {
                    string name = binder.CallInfo.ArgumentNames.Count > i ?
                        binder.CallInfo.ArgumentNames[i] : null;
                    if (name == "queryParams")
                    {
                        queryParams = obj.ToString();
                    }
                    else if (name == "requestBody")
                    {
                        requestBody = obj.ToString();
                    }
                    else if (name == "requestHeaders")
                    {
                        AddRequestHeader((Dictionary<string, string>)obj);
                    }
                    else if (name == "cancellationToken")
                    {
                        cancellationToken = (CancellationToken)obj;
                    }
                    i++;
                }
                result = RequestAsync(binder.Name.ToUpper(), requestBody: requestBody, queryParams: queryParams, cancellationToken: cancellationToken).ConfigureAwait(false);
                return true;
            }
            else
            {
                result = null;
                return false;
            }

        }

        /// <summary>
        ///     Make the call to the API server, override for testing or customization
        /// </summary>
        /// <param name="client">Client object ready for communication with API</param>
        /// <param name="request">The parameters for the API call</param>
        /// <param name="cancellationToken">A token that allows cancellation of the http request</param>
        /// <returns>Response object</returns>
        public virtual async Task<ResponseDto> MakeRequest(HttpClient client, HttpRequestMessage request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await client.SendAsync(request, cancellationToken).ConfigureAwait(false);
            var responseCode = (int)response.StatusCode;
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseHeaders = response.Headers.ToDictionary(header => header.Key, header => header.Value);
            return new ResponseDto(responseCode, responseContent, responseHeaders);
        }

        /// <summary>
        ///     Prepare for async call to the API server
        /// </summary>
        /// <param name="method">HTTP verb</param>
        /// <param name="cancellationToken">A token that allows cancellation of the http request</param>
        /// <param name="requestBody">JSON formatted string</param>
        /// <param name="queryParams">JSON formatted queary paramaters</param>
        /// <returns>Response object</returns>
        private async Task<ResponseDto> RequestAsync(string method, String requestBody = null, String queryParams = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Build the URL
                    client.BaseAddress = new Uri(Host);
                    client.Timeout = Timeout;
                    string endpoint = BuildUrl(queryParams);


                    // Build the request headers
                    client.DefaultRequestHeaders.Accept.Clear();
                    if (RequestHeaders != null)
                    {
                        foreach (KeyValuePair<string, string> header in RequestHeaders)
                        {
                            switch (header.Key)
                            {
                                case "Authorization":
                                    client.DefaultRequestHeaders.Authorization = AddAuthorization(header);
                                    break;
                                case "Content-Type":
                                    MediaType = header.Value;
                                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaType));
                                    break;
                                default:
                                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                                    break;
                            }
                        }
                    }

                    // Build the request body
                    StringContent content = null;
                    if (requestBody != null)
                    {
                        content = new StringContent(requestBody, Encoding.UTF8, MediaType);
                    }

                    // Build the final request
                    HttpRequestMessage request = new HttpRequestMessage
                    {
                        Method = new HttpMethod(method),
                        RequestUri = new Uri(endpoint),
                        Content = content
                    };
                    return await MakeRequest(client, request, cancellationToken).ConfigureAwait(false);

                }
                catch (TaskCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    var response = new HttpResponseMessage();
                    var message = (ex is HttpRequestException) ? ".NET HttpRequestException" : ".NET Exception";
                    message = message + ", raw message: \n\n";
                    var responseCode = (int)response.StatusCode;
                    var responseContent = message + ex.Message;
                    var responseHeaders = response.Headers.ToDictionary(header => header.Key, header => header.Value);
                    return new ResponseDto(responseCode, responseContent, responseHeaders);
                }
            }
        }
    }
}
