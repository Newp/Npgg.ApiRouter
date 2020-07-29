using System;
using System.Net.Http;



namespace Npgg.ApiRouter
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class HttpAttribute : Attribute
    {
        public readonly HttpMethod Method;
        public readonly string Pattern;
        public HttpAttribute(HttpMethod method, string pattern)
        {
            this.Method = method;
            this.Pattern = pattern;
        }
        public HttpAttribute(HttpMethod method) : this(method, string.Empty)
        {

        }
    }
}
