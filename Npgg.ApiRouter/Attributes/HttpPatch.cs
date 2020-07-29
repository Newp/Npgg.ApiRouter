using System.Net.Http;

namespace Npgg.ApiRouter.Attributes
{
    public class HttpPatch : HttpAttribute
    {
        public HttpPatch() : this(null) { }

        public HttpPatch(string pattern) : base(HttpMethod.Patch, pattern)
        {
        }
    }
}