using System.Net.Http;



namespace Npgg.ApiRouter
{
    public class HttpPost : HttpAttribute
    {
        public HttpPost() : this(null) { }

        public HttpPost(string pattern) : base(HttpMethod.Post, pattern)
        {
        }
    }

}
