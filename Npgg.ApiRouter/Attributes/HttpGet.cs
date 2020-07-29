using System.Net.Http;



namespace Npgg.ApiRouter
{
    public class HttpGet : HttpAttribute
    {
        public HttpGet() : this(null) { }

        public HttpGet(string pattern) : base(HttpMethod.Get, pattern)
        {

        }
    }

}
