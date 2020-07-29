using System;



namespace Npgg.ApiRouter
{
    public class FromBodyAttribute : Attribute
    {
    }
    public class FromQueryAttribute : Attribute
    {
        public string? Alias { get; }

        public FromQueryAttribute()
        {
        }

        public FromQueryAttribute(string alias)
        {
            this.Alias = alias;
        }
    }

}
