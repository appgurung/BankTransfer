
using BankTransfer.Core.Interface;
using BankTransfer.Core.Models.Request.CBA;
using BankTransfer.Core.Models.Response.CBA;
using RestSharp;
using RestSharp.Authenticators;
using System.Net;

namespace BankTransfer.Core.Helpers
{
    public class RestClientHelper : IRestClientHelper
    {

        public RestClientHelper()
        {

        }

        private string _baseUrl = "";
        public void SetBaseUrl(string url)
        {
            _baseUrl = url;
        }
        public async Task<HttpResponse> Post(string relativeUrl, string? payLoad, CancellationToken token)
        {
            var client = new RestClient(string.Concat(_baseUrl));

            var request = new RestRequest(relativeUrl, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", payLoad, ParameterType.RequestBody);
            RestResponse response = await client.ExecuteAsync(request, token);
            return new HttpResponse()
            {
                content = response.Content!,
                IsSuccessful = response.IsSuccessful,
                statusCode = response.StatusCode,
                IsSuccessStatusCode = response.IsSuccessStatusCode
            };
        }

        public async Task<HttpResponse> Post(string relativeUrl, bool hasHeader, Dictionary<string, string> hederValues, string? payLoad, CancellationToken token)
        {
            if (!hasHeader)
                return await this.Post(relativeUrl, payLoad, token);

            var client = new RestClient(_baseUrl);

            var request = new RestRequest(relativeUrl, Method.Post);

            foreach (KeyValuePair<string, string> item in hederValues)
            {
                request.AddHeader(item.Key, item.Value);
            }

            if (!string.IsNullOrEmpty(payLoad))
                request.AddParameter("application/json", payLoad, ParameterType.RequestBody);

            RestResponse response = await client.ExecuteAsync(request, token);
            return new HttpResponse()
            {
                content = response.Content!,
                IsSuccessful = response.IsSuccessful,
                statusCode = response.StatusCode,
                IsSuccessStatusCode = response.IsSuccessStatusCode
            };
        }
        public async Task<HttpResponse> Post(string relativeUrl, bool hasHeader, Dictionary<string, string> hederValues, string? payLoad, BasicAuthenticationRequest authReq, CancellationToken token)
        {

            if (!hasHeader)
                return await this.Post(relativeUrl, payLoad, token);

            var client = new RestClient(_baseUrl);

            var request = new RestRequest(relativeUrl, Method.Post);

            foreach (KeyValuePair<string, string> item in hederValues)
            {
                request.AddHeader(item.Key, item.Value);
            }
            if (authReq != null)
                client.Authenticator = new HttpBasicAuthenticator(authReq.username, authReq.password);

            if (!string.IsNullOrEmpty(payLoad))
                request.AddParameter("application/json", payLoad, ParameterType.RequestBody);

            RestResponse response = await client.ExecuteAsync(request, token);
            return new HttpResponse()
            {
                content = response.Content!,
                IsSuccessful = response.IsSuccessful,
                statusCode = response.StatusCode,
                IsSuccessStatusCode = response.IsSuccessStatusCode
            };

        }

        public async Task<HttpResponse> Get(string relativeUrl, bool hasHeader, Dictionary<string, string> hederValues, CancellationToken token)
        {
            if (!hasHeader)
                return await this.Get(relativeUrl);

            var client = new RestClient(string.Concat(_baseUrl));

            var request = new RestRequest(relativeUrl, Method.Get);
            foreach (KeyValuePair<string, string> item in hederValues)
            {
                request.AddHeader(item.Key, item.Value);
            }
            request.AddHeader("Content-Type", "application/json");
            RestResponse response = await client.GetAsync(request, token);
            return new HttpResponse()
            {
                content = response.Content!,
                IsSuccessful = response.IsSuccessful,
                statusCode = response.StatusCode,
                IsSuccessStatusCode = response.IsSuccessStatusCode
            };
        }

        public async Task<HttpResponse> Get(string relativeUrl, bool hasHeader, Dictionary<string, string> hederValues, params string[] data)
        {
            if (!hasHeader)
                return await this.Get(relativeUrl, data);

            string url = (string.Concat(_baseUrl));
            int a = 0;
            string f = null;
            if (data != null && data.Count() > 0)
            {
                foreach (var d in data.ToList())
                {
                    f = d;
                    if (data.Count() == 1)
                    {
                        url += (string.Concat(relativeUrl, d));
                    }
                    else if ((data.Count() > 1) == true && a == 0)
                    {
                        a = 1;
                        url = url + relativeUrl + d;
                    }
                    else if ((data.Count() > 1) == true && a > 0)
                    {
                        url = url + "&" + $"{f.Replace("?", "&")}";
                    }
                }
            }
            else
            {
                url += relativeUrl;
            }

            var client = new RestClient(url);

            var request = new RestRequest(url, Method.Get);
            foreach (KeyValuePair<string, string> item in hederValues)
            {
                request.AddHeader(item.Key, item.Value);
            }
            request.AddHeader("Content-Type", "application/json");
            RestResponse response = await client.ExecuteAsync(request);
            return new HttpResponse()
            {
                content = response.Content!,
                IsSuccessful = response.IsSuccessful,
                statusCode = response.StatusCode,
                IsSuccessStatusCode = response.IsSuccessStatusCode
            };

        }

        public async Task<HttpResponse> Get(string relativeUrl, params string[] data)
        {
            string url = (string.Concat(_baseUrl));
            int a = 0;
            string f = null!;
            if (data != null)
            {
                foreach (var d in data.ToList())
                {
                    f = d;
                    if (data.Count() == 1)
                    {
                        url = (string.Concat(relativeUrl, d));
                    }
                    else if ((data.Count() > 1) == true && a == 0)
                    {
                        a = 1;
                        url = url + relativeUrl + d;
                    }
                    else if ((data.Count() > 1) == true && a > 0)
                    {
                        url = url + "&" + $"{f.Replace("?", "&")}";
                    }
                }
            }

            var client = new RestClient(_baseUrl);

            var request = new RestRequest(url, Method.Get);
            request.AddHeader("Content-Type", "application/json");
            RestResponse response = await client.ExecuteAsync(request);
            return new HttpResponse()
            {
                content = response.Content!,
                IsSuccessful = response.IsSuccessful,
                statusCode = response.StatusCode,
                IsSuccessStatusCode = response.IsSuccessStatusCode
            };
        }


        public async Task<HttpResponse> Delete(string relativeUrl, CancellationToken token)
        {
            var client = new RestClient(string.Concat(_baseUrl));

            var request = new RestRequest(relativeUrl, Method.Delete);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", ParameterType.RequestBody);
            RestResponse response = await client.ExecuteAsync(request, token);
            return new HttpResponse()
            {
                content = response.Content!,
                IsSuccessful = response.IsSuccessful,
                statusCode = response.StatusCode,
                IsSuccessStatusCode = response.IsSuccessStatusCode
            };
        }
        public async Task<HttpResponse> Put(string relativeUrl, CancellationToken token)
        {
            var client = new RestClient(string.Concat(_baseUrl));

            var request = new RestRequest(relativeUrl, Method.Put);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", ParameterType.RequestBody);
            RestResponse response = await client.ExecuteAsync(request, token);
            return new HttpResponse()
            {
                content = response.Content!,
                IsSuccessful = response.IsSuccessful,
                statusCode = response.StatusCode,
                IsSuccessStatusCode = response.IsSuccessStatusCode
            };
        }
        public HttpResponse Post(string relativeUrl, string payLoad, ref HttpStatusCode statusCode, CancellationToken token)
        {

            var client = new RestClient(_baseUrl);

            var request = new RestRequest(relativeUrl, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", payLoad, ParameterType.RequestBody);
            RestResponse response = client.Execute(request, token);
            statusCode = response.StatusCode;
            return new HttpResponse()
            {
                content = response.Content!,
                IsSuccessful = response.IsSuccessful,
                statusCode = response.StatusCode,
                IsSuccessStatusCode = response.IsSuccessStatusCode
            };
        }

        public async Task<HttpResponse> Put(string relativeUrl, string payLoad, CancellationToken token)
        {
            var client = new RestClient(string.Concat(_baseUrl));

            var request = new RestRequest(relativeUrl, Method.Put);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", payLoad, ParameterType.RequestBody);
            RestResponse response = await client.ExecuteAsync(request, token);
            return new HttpResponse()
            {
                content = response.Content!,
                IsSuccessful = response.IsSuccessful,
                statusCode = response.StatusCode,
                IsSuccessStatusCode = response.IsSuccessStatusCode
            };
        }

        public async Task<HttpResponse> Patch(string relativeUrl, string payLoad, CancellationToken token)
        {

            var client = new RestClient(string.Concat(_baseUrl));

            var request = new RestRequest(relativeUrl, Method.Patch);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", payLoad, ParameterType.RequestBody);
            RestResponse response = await client.ExecuteAsync(request, token);
            return new HttpResponse()
            {
                content = response.Content!,
                IsSuccessful = response.IsSuccessful,
                statusCode = response.StatusCode,
                IsSuccessStatusCode = response.IsSuccessStatusCode
            };
        }

    }
}