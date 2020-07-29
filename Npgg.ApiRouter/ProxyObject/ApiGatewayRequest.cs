using System;
using System.Collections.Generic;
using System.Text;

namespace Npgg.ApiRouter

{


    public class APIGatewayRequest
    {
        public string Path { get; set; }
        public string HttpMethod { get; set; }
        
        //public IDictionary<string, string> Headers { get; set; }
        public HeaderCollection MultiValueHeaders { get; set; }
        public QueryStringCollection MultiValueQueryStringParameters { get; set; }
        
        
        
        public string Body { get; set; }

        public bool IsBase64Encoded { get; set; }

    }
}
