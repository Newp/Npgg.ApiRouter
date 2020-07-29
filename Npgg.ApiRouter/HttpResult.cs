using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace Npgg.ApiRouter
{

    public class HttpResult
    {
        //for deserializer
        public HttpResult()
        {
        }

        public HttpResult(HttpStatusCode statusCode, object body)
        {
            StatusCode = statusCode;
            Body = body;
        }


        public bool IsSuccess
            => StatusCode == HttpStatusCode.OK
               || StatusCode == HttpStatusCode.Created
               || StatusCode == HttpStatusCode.Accepted;


        [JsonProperty("status_code")] public HttpStatusCode StatusCode { get; set; }

        [JsonProperty("body")] public object Body { get; set; }

        [JsonProperty("response_header")] public IDictionary<string, string> ResponseHeader { get; set; }
    }
}