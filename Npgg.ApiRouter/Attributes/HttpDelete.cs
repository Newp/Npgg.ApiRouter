using System.Net.Http;



namespace Npgg.ApiRouter
{
    public class HttpDelete : HttpAttribute
    {
        public HttpDelete() : this(null) { }

        public HttpDelete(string pattern) : base(HttpMethod.Delete, pattern)
        {
        }
    }

}
