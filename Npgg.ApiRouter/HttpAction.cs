using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using System.Reflection;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;
using Microsoft.VisualBasic;

//using System.Text.Json;

namespace Npgg.ApiRouter
{

    public class HttpAction
    {
        public readonly Type ControllerType;
        public readonly string HttpMethod;
        public readonly string PathPattern;
        public readonly MethodInfo MethodInfo;
        public readonly MethodInvoker Invoker;
        public readonly InjectInfo BodyInjectInfo = null;
        public readonly Dictionary<string, InjectInfo> QueryInjectInfo = new Dictionary<string, InjectInfo>();
        readonly int _paramterLength = -1;
        public readonly List<ParameterMappingInfo> MappingInfoList = new List<ParameterMappingInfo>();

        

        public HttpAction(Type controllerType,  string Pattern, MethodInfo method, string httpMethod)
        {
            this.ControllerType = controllerType;
            this.HttpMethod = httpMethod;
            this.PathPattern = Pattern;
            this.MethodInfo = method;
            this.Invoker = new MethodInvoker(method);

            var plist = method.GetParameters();
            _paramterLength = plist.Length;
            int length = plist.Length;

            var splited = Pattern.Split('/').ToList();

            for (int actionParameterIndex = 0; actionParameterIndex < plist.Length; actionParameterIndex++)
            {
                var p = plist[actionParameterIndex];

                if (p.GetCustomAttribute<FromBodyAttribute>() != null)
                {
                    BodyInjectInfo = new InjectInfo
                    {
                        Index= actionParameterIndex
                    };

                    BodyInjectInfo.Set(p.Name, p.ParameterType);

                    continue;
                }


                var fromQuery = p.GetCustomAttribute<FromQueryAttribute>();

                if(fromQuery != null)
                {
                    var name=p.Name;
                    if (fromQuery.Alias!=null)
                    {
                        name = fromQuery.Alias;
                    }

                    var queryInjectInfo = new InjectInfo()
                    {
                        Index = actionParameterIndex,
                        Converter = TypeDescriptor.GetConverter(p.ParameterType),
                    };
                    queryInjectInfo.Set(p.Name, p.ParameterType);
                    QueryInjectInfo.Add(name, queryInjectInfo);

                    continue;
                }
                
                {
                    string pname = '{' + p.Name + '}';

                    if (splited.Contains(pname) == false)
                    {
                        throw new Exception("url pattern not contains parameter =>" + pname);
                    }

                    var parameterMappingInfo = new ParameterMappingInfo()
                    {
                        PatternIndex = splited.IndexOf(pname),
                        Converter = TypeDescriptor.GetConverter(p.ParameterType),
                        ParameterIndex = actionParameterIndex
                    };

                    parameterMappingInfo.Set(p.Name, p.ParameterType);

                    MappingInfoList.Add(parameterMappingInfo);
                }
            }

        }

        Dictionary<Type, Attribute> customAttributes = new Dictionary<Type, Attribute>();
        public T GetCustomAttribute<T>() where T : Attribute
        {
            var type = typeof(T);

            if(customAttributes.TryGetValue(type, out var cached))
            {
                //cache hit시 바로 return
                return (T)cached;
            }

            var result = this.MethodInfo.GetCustomAttribute<T>();
            customAttributes.Add(type, result); // cache, null이어도 넣는다
            return result;
        }

        

        public object[] MakeParamters(string path, string body, IDictionary<string, ICollection<string>> queryStrings)
        {
            object[] result = new object[this._paramterLength];

            var splitedPath = path.Split('/');

            foreach (var mappingInfo in MappingInfoList)
            {
                string value = splitedPath[mappingInfo.PatternIndex];
                result[mappingInfo.ParameterIndex] = mappingInfo.Converter.ConvertFromString(value);
            }

            if (BodyInjectInfo != null)
            {
                try
                {
                    if (BodyInjectInfo.Type.IsAssignableFrom(typeof(string)))
                    {
                        result[BodyInjectInfo.Index] = body;
                    }
                    else
                    {
                        result[BodyInjectInfo.Index] = JsonConvert.DeserializeObject(body, BodyInjectInfo.Type);    
                    }
                }
                catch (Exception ex)
                {
                    throw new HandledException(HttpStatusCode.BadRequest, ex.Message);
                }
            }

            if(QueryInjectInfo.Count > 0)
            {
                foreach(var kvp in QueryInjectInfo)
                {
                    var injectInfo = kvp.Value;
                    var queryStringKey = kvp.Key;
                    var value = queryStrings[queryStringKey].ToList();


                    if (value.Count == 0)
                        throw new HandledException(HttpStatusCode.BadRequest, "querystring parameter is 0=>" + queryStringKey);

                    result[injectInfo.Index] = injectInfo.Converter.ConvertFromString(value[0]);
                }
            }

            return result;
        }


        public async Task<object> Invoke(IServiceProvider scopedProvider, string path, string body, QueryStringCollection queryStrings)
        {

            var parameter = this.MakeParamters(path.Split('?')[0], body, queryStrings);
            object controller = scopedProvider.GetService(this.ControllerType);

            object result = null;


            #region invoke
            switch (this.Invoker.InvokerType)
            {
                case InvokerType.AsyncReturnWithArguments:
                    result = await this.Invoker.InvokeReturnAsync(controller, parameter);
                    break;
                case InvokerType.ReturnWithArguments:
                    result = this.Invoker.InvokeReturn(controller, parameter);
                    break;
                case InvokerType.Return:
                    result = this.Invoker.ParameterlessInvokeReturn(controller);
                    break;
                case InvokerType.AsyncReturn:
                    result = await this.Invoker.InvokeReturnAsync(controller);
                    break;

                case InvokerType.VoidWithArguments:
                    this.Invoker.InvokeVoid(controller, parameter);
                    break;

                case InvokerType.AsyncVoidWithArguments:
                    this.Invoker.InvokeVoidAsync(controller, parameter);
                    break;
                case InvokerType.AsyncVoid:
                case InvokerType.Void:
                    this.Invoker.InvokeVoid(controller);
                    break;
                default:
                    result = null;
                    break;
            }
            #endregion

            return result;
        }


        
    }

    public class ParameterInfo
    {
        public string Name { get; private set; }
        public Type Type { get; private set; }

        public void Set(string name, Type type)
        {
            this.Name = name;
            this.Type = type;
        }
    }


    public class InjectInfo : ParameterInfo
    {
        public int Index { get; set; }
        public TypeConverter Converter { get; set; }
    }
}
