using System;
using System.Collections.Generic;

using System.Reflection;

namespace Npgg.ApiRouter
{
    public static class RouteHelper
    {
        public static bool IsParameter(string block) => block.StartsWith('{') && block.EndsWith('}');

        public static string GetControllerPrepath(Type controllerType)
        {
            var route = controllerType.GetCustomAttribute<ControllerAttribute>();

            if (route == null)
                return null;

            string prepath = route.UrlPattern.ToLower();

            if (prepath.Contains("[controller]"))
            {
                if (controllerType.Name.ToLower().EndsWith("controller"))
                {
                    string ctrlName = controllerType.Name.ToLower().Replace("controller", string.Empty);
                    prepath = prepath.Replace("[controller]", ctrlName);
                }
                else
                {
                    throw new Exception("invalid controller name");
                }    

            }

            return prepath;
        }

        public static List<HttpAction> GetActions(Type controllerType)
        {
            string prepath = GetControllerPrepath(controllerType);

            if (prepath == null)
                return null;

            List<HttpAction> result = new List<HttpAction>();

            foreach (var method in controllerType.GetMethods())
            {
                var httpList = method.GetCustomAttributes<HttpAttribute>();

                foreach (var http in httpList)
                {
                    string path = string.IsNullOrEmpty(http.Pattern) 
                        ? String.Join('/', http.Method.ToString().ToLower(), prepath)
                        : String.Join('/', http.Method.ToString().ToLower(), prepath, http.Pattern);

                    path = path.Replace("//", "/");
                    string httpMethod = http.Method.ToString();
                    HttpAction action = new HttpAction(controllerType, path, method, httpMethod);

                    result.Add(action);
                }
            }

            return result;
        }
    }
}
