using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Npgg.ApiRouter
{

    public enum InvokerType
    {
        Void = 1,
        VoidWithArguments,
        Return,
        ReturnWithArguments,

        AsyncVoid = 11,
        AsyncVoidWithArguments,
        AsyncReturn,
        AsyncReturnWithArguments,
    }

    public class MethodInvoker
    {
        private delegate void VoidDelegate(object instance, object[] arguments);
        private delegate void ParameterlessVoidDelegate(object instance);
        private delegate object ParameterlessReturnValueDelegate(object instance);
        private delegate object ReturnValueDelegate(object instance, object[] arguments);

        //public bool IsBinaryMethod { get; private set; }
        public bool IsAsyncMethod { get; private set; }
        public readonly bool IsVoid;

        public InvokerType InvokerType { get; private set; }
        public Type valueType { get; private set; }

        static readonly Type _taskType = typeof(Task);

        public MethodInvoker( MethodInfo methodInfo)
        {
            var instanceExpression = Expression.Parameter(typeof(object), "instance");
            var argumentsExpression = Expression.Parameter(typeof(object[]), "arguments");
            var argumentExpressions = new List<Expression>();
            var parameterInfos = methodInfo.GetParameters();

            bool haveParameter = parameterInfos.Length > 0;

            if (haveParameter)
            {
                valueType = parameterInfos.Last().ParameterType;

                for (var i = 0; i < parameterInfos.Length; ++i)
                {
                    var parameterInfo = parameterInfos[i];
                    argumentExpressions.Add(
                        Expression.Convert(Expression.ArrayIndex(argumentsExpression, Expression.Constant(i)), parameterInfo.ParameterType));
                }
            }
            else
            {
                this.valueType = null;
            }

            var callExpression = Expression.Call(Expression.Convert(instanceExpression, methodInfo.ReflectedType), methodInfo, argumentExpressions);

            if (callExpression.Type == typeof(void) || callExpression.Type == _taskType)
            {
                IsVoid = true;
                if (haveParameter)
                {
                    InvokerType = InvokerType.VoidWithArguments;
                    PVDelegate = Expression.Lambda<VoidDelegate>(callExpression, instanceExpression, argumentsExpression).Compile();
                }
                else
                {
                    InvokerType = InvokerType.Void;
                    VDelegate = Expression.Lambda<ParameterlessVoidDelegate>(callExpression, instanceExpression).Compile();
                }
            }
            else
            {
                if (haveParameter)
                {
                    InvokerType = InvokerType.ReturnWithArguments;
                    RDelegate = Expression.Lambda<ReturnValueDelegate>(Expression.Convert(callExpression, typeof(object)), instanceExpression, argumentsExpression).Compile();
                }
                else
                {
                    InvokerType = InvokerType.Return;
                    PRDelegate = Expression.Lambda<ParameterlessReturnValueDelegate>(Expression.Convert(callExpression, typeof(object)), instanceExpression).Compile();
                }
            }


            //_taskType.IsAssignableFrom(callExpression.Type) && callExpression.Type != _taskType;

            var asyncAttr = methodInfo.GetCustomAttribute(typeof(AsyncStateMachineAttribute));

            this.IsAsyncMethod = asyncAttr != null;

            if(IsAsyncMethod)
            {
                InvokerType += 10;
            }
        }

        private VoidDelegate PVDelegate;
        private ParameterlessVoidDelegate VDelegate;
        private ReturnValueDelegate RDelegate;
        private ParameterlessReturnValueDelegate PRDelegate;

        public void InvokeVoid(object Instance)
        {
            VDelegate(Instance);
        }

        public void InvokeVoid(object Instance, object[] arguments)
        {
            PVDelegate(Instance, arguments);
        }

        public async void InvokeVoidAsync(object Instance, object[] arguments)
        {
            PVDelegate(Instance, arguments);
            await Task.CompletedTask;
        }

        //

        public object ParameterlessInvokeReturn(object Instance)
        {
            return PRDelegate(Instance);
        }

        public object InvokeReturn(object Instance, object[] arguments)
        {
            return RDelegate(Instance, arguments);
        }

        //

        public async Task<object> InvokeReturnAsync(object Instance)
        {
            return await (dynamic)PRDelegate(Instance);
        }
        public async Task<object> InvokeReturnAsync(object Instance, object[] arguments)
        {
            return await (dynamic)RDelegate(Instance, arguments);
        }
    }

}
