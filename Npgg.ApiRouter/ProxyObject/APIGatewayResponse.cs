using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Npgg.ApiRouter
{
    public class APIGatewayResponse
    {

        //
        // 요약:
        //     The HTTP status code for the request
        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }

        [JsonProperty("headers")]
        public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        //
        // 요약:
        //     The response body
        [JsonProperty("body")]
        public string Body { get; set; }
        //
        // 요약:
        //     Flag indicating whether the body should be treated as a base64-encoded string
        [JsonProperty("isBase64Encoded")]
        public bool IsBase64Encoded { get; set; }
    }
}
