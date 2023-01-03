using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BankTransfer.Core.Models.Response.CBA
{
    public class HttpResponse
    {
        public HttpStatusCode statusCode { get; set; }
        public bool IsSuccessful { get; set; }
        public bool IsSuccessStatusCode { get; set; }
        public string? content { get; set; }
    }
}