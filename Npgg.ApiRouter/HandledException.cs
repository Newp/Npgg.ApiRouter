using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Npgg.ApiRouter
{
    public class HandledException : Exception
    {
        public HandledException([NotNull] HttpStatusCode statusCode, [NotNull] string body)
        {
            StatusCode = statusCode;
            Body = body;
        }

        public HandledException([NotNull] HttpStatusCode statusCode, [NotNull] string body, Exception e) : base(body.ToString(), e)
        {
            StatusCode = statusCode;
            Body = body;
        }

        [JsonProperty("status_code")] public HttpStatusCode StatusCode { get; set; }

        [JsonProperty("body")] public string Body { get; set; }
    }
}