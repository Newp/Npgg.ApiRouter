using System.Net.Http;



namespace Npgg.ApiRouter
{
    public class HttpPut : HttpAttribute
    {
        public HttpPut() : this(null) { }

        public HttpPut(string pattern) : base(HttpMethod.Put, pattern)
        {
        }
    }

}
