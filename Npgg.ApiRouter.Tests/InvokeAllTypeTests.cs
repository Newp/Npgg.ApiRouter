using Microsoft.Extensions.DependencyInjection;
using Npgg.ApiRouter.Tests.TestController;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Npgg.ApiRouter.Tests
{
    public class InvokeAllTypeTests
    {
        readonly Router router = new Router();
        readonly IServiceProvider _provider = null;
        public InvokeAllTypeTests()
        {
            AllInvokeController.LastInvoked = null;

            var _controller = new AllInvokeController();

            var _list = RouteHelper.GetActions(_controller.GetType());

            Assert.Equal(8, _list.Count);
            ServiceCollection services = new ServiceCollection();
            router.Regist(services, _list);

            this._provider = services.BuildServiceProvider();
        }


        [Theory]
        [InlineData("/api/AllInvoke/Async", "aaaa3939")]
        [InlineData("/api/allinvoke/async", "aaaa3939")] //대소문자 구분
        [InlineData("/api/AllInvoke/Sync", "aaaabbbb")]
        public void EchoTest(string baseUrl, string input)
        {
            string url = baseUrl + "/" + input;

            var action = router.GetAction("get", url);
            Assert.NotNull(action);

            var result = action.Invoke(_provider, url, null, null).Result;
            Assert.NotNull(result);
            string output = result as string;
            Assert.NotNull(output);

            Assert.StartsWith("echo:", output);
            Assert.Equal(input, output.Substring(5));
        }

        [Theory]
        [InlineData("/api/AllInvoke/SyncParameterless", "SyncParameterless")]
        [InlineData("/api/AllInvoke/AsyncParameterless", "AsyncParameterless")]
        public void ParameterlessReturnTest(string url, string expectOutput)
        {
            var action = router.GetAction("get", url);
            Assert.NotNull(action);

            var result = action.Invoke(_provider, url, null, null).Result;
            Assert.NotNull(result);
            string output = result as string;
            Assert.NotNull(output);
            Assert.Equal(expectOutput, output);
        }


        [Theory]
        [InlineData("/api/AllInvoke/AsyncParameterlessVoid", "AsyncParameterlessVoid")]
        [InlineData("/api/AllInvoke/SyncParameterlessVoid", "SyncParameterlessVoid")]
        public void ParameterlessVoidTest(string url, string expectInvoked)
        {
            Assert.Null(AllInvokeController.LastInvoked);
            var action = router.GetAction("get", url);
            Assert.NotNull(action);

            var result = action.Invoke(_provider, url, null, null).Result;
            Assert.Null(result);
            Assert.NotNull(AllInvokeController.LastInvoked);
            Assert.Equal(expectInvoked, AllInvokeController.LastInvoked);
        }


        [Theory]
        [InlineData("/api/AllInvoke/SyncVoid", "adfadsffwvevw")]
        [InlineData("/api/AllInvoke/AsyncVoid", "tesdf123412231")]
        public void VoidTest(string baseUrl, string input)
        {
            string url = baseUrl + "/" + input;

            var action = router.GetAction("get", url);
            Assert.NotNull(action);

            var result = action.Invoke(_provider, url, null, null).Result;
            Assert.Null(result);
            Assert.NotNull(AllInvokeController.LastInvoked);
            Assert.Equal(input, AllInvokeController.LastInvoked);
        }

        
    }

}
