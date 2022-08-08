using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace svcGangManagement.Utility
{
    public class WebAPIClient
    {
        private string LocalApplication = "svcGangManagement";
        private HttpClient httpClient = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });

        public string BaseURL { get; set; }

        public WebAPIClient(string baseURL, string racf)
        {
            BaseURL = baseURL;

            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("RacfId", racf);
            httpClient.DefaultRequestHeaders.Add("Source", LocalApplication);
        }

        public R GetEndpoint<R>(string route)
        {
            return Endpoint<R>(route, url => httpClient.GetAsync(url));
        }

        public R PostEndpoint<T, R>(string route, T content)
        {
            return Endpoint<R>(route, url => httpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json")));
        }

        public R Endpoint<R>(string route, Func<Uri, Task<HttpResponseMessage>> apiCall)
        {
            try
            {
                var url = new Uri(BaseURL + "/" + Uri.EscapeUriString(route));

                var response = apiCall(url).Result;
                if (response != null)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var json = response.Content.ReadAsAsync<JsonResponse<R>>().Result;
                        return json.Result;
                    }
                    else
                        HandleError(route, response.StatusCode);
                }
            }
            catch (AggregateException ex)
            {

            }
            catch (Exception)
            {

            }

            return default(R);
        }

        public static void HandleError(string route, HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.InternalServerError)
                return; // Service has already logged and e-mailed the exception

            string message = (int)statusCode + " " + HttpWorkerRequest.GetStatusDescription((int)statusCode);

            var ex = new Exception(route + ": " + message);
            try
            {
                throw ex;
            }
            catch (Exception ex2)
            {
                // log ex
            }
        }
    }
}