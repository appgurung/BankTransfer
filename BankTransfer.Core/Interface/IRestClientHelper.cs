using BankTransfer.Core.Helpers;
using BankTransfer.Core.Models.Request.CBA;
using BankTransfer.Core.Models.Response.CBA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransfer.Core.Interface
{
    public interface IRestClientHelper
    {
        void SetBaseUrl(string url);
        Task<HttpResponse> Post(string relativeUrl, string? payLoad, CancellationToken token);
        Task<HttpResponse> Post(string relativeUrl, bool hasHeader, Dictionary<string, string> hederValues, string? payLoad, CancellationToken token);
        Task<HttpResponse> Post(string relativeUrl, bool hasHeader, Dictionary<string, string> hederValues, string? payLoad, BasicAuthenticationRequest authReq, CancellationToken token);
        HttpResponse Post(string absoluteUrl, string payLoad, ref System.Net.HttpStatusCode statusCode, CancellationToken token);
        Task<HttpResponse> Get(string relativeUrl, bool hasHeader, Dictionary<string, string> hederValues, CancellationToken token);
        Task<HttpResponse> Get(string relativeUrl, params string[] data);
        Task<HttpResponse> Get(string relativeUrl, bool hasHeader, Dictionary<string, string> hederValues, params string[] data);
        Task<HttpResponse> Delete(string relativeUrl, CancellationToken token);
        Task<HttpResponse> Put(string path, CancellationToken token);
        Task<HttpResponse> Put(string path, string payLoad, CancellationToken token);
        Task<HttpResponse> Patch(string relativeUrl, string payLoad, CancellationToken token);
    }
}
