using System;
using System.Collections.Generic;

using System.Reflection;
using System.ComponentModel;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Npgg.ApiRouter
{
    public class RouterMap: Dictionary<string, HttpAction>
    {
        
    }
    public class Router
    {
        readonly RouterNode<HttpAction> _rootNode = null;
        public Router()
        {
            _rootNode = new RouterNode<HttpAction>();
        }

        public List<HttpAction> Regist(IServiceCollection services, Assembly assembly)
        {
            List<HttpAction> result = new List<HttpAction>();

            var list= assembly.GetTypes().Where( type=> type.GetCustomAttribute<ControllerAttribute>() != null).ToList();

            foreach(var controllerType in list)
            {
                var actionList = RouteHelper.GetActions(controllerType);
                this.Regist(services, actionList);
                result.AddRange(actionList);
            }
            
            services.AddSingleton(GetRouterMap());

            return result;
        }

        public void Regist(IServiceCollection services, List<HttpAction> list)
        {
            foreach (var action in list)
            {
                string[] block = ( action.PathPattern).Split('/');
                _rootNode.Add(block, 0, action);
                services.AddScoped(action.ControllerType);
            }
        }

        public HttpAction GetAction(string method, string path)
        {
            if (path.StartsWith('/') == false)
                path = '/' + path;

            string routePath = method + path;
            routePath = routePath.Split('?')[0].ToLower();
            if (_rootNode.TryGet(routePath.Split('/'), 0, out var result) == false)
            {
                return null;
            }

            return result.Value;
        }

        private RouterMap GetRouterMap()
        {
            var result = new RouterMap();
            _rootNode.ReadNode(result);

            return result;
        }
    }


    public class ControllerAttribute : Attribute
    {
        public ControllerAttribute(string urlPattern)
        {
            this.UrlPattern = urlPattern;
        }
        public ControllerAttribute() : this(string.Empty)
        {
        }
        public string Path { get; set; }
        public string UrlPattern { get; set; }
        //public string Path { get; set; }
    }


    public class ParameterMappingInfo : ParameterInfo
    {
        public int PatternIndex { get; set; }
        public TypeConverter Converter { get; set; }
        public int ParameterIndex { get; set; }
    }
}
