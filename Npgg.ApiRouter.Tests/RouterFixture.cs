using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Npgg.ApiRouter.Tests
{
    public class RouterFixture
    {

        protected readonly Router _router = new Router();
        readonly IServiceProvider _provider;
        public RouterFixture()
        {
            ServiceCollection services = new ServiceCollection();

            var controllers = new Type[]
            {
                typeof(MultiParameteredPathTestController)
            };

            foreach(var controllerType in controllers)
            {
                var actions = RouteHelper.GetActions(controllerType);

                _router.Regist(services, actions);
            }

            _provider = services.BuildServiceProvider();
        }


        string MakePath(Type controllerType, params object[] args)
        {
            string prepath = RouteHelper.GetControllerPrepath(controllerType);
            List<object> list = new List<object>(args);
            list.Insert(0, prepath);
            var result = '/' + string.Join('/', list);

            return result;
        }

        [Fact]

        public void Test1()
        {
            var type = typeof(MultiParameteredPathTestController);

            string p1 = "test1";
            int p2 = 39;
            float p3 = 39.3939f;


            string path = MakePath(type, p1, p2, p3);

            var action = _router.GetAction("get", path);
            Assert.NotNull(action);
            Assert.Equal(type, action.ControllerType);

            var controller = _provider.GetService(action.ControllerType) as MultiParameteredPathTestController;

            Assert.NotNull(controller);
            Assert.Equal(type, controller.GetType());


            var invoker = action.Invoker;

            Assert.False(action.Invoker.IsAsyncMethod);

            var paramters = action.MakeParamters(path, null, null);
            var result = invoker.InvokeReturn(controller, paramters);

            Assert.NotNull(result);

            var response = result as MultiParameteredPathTestController.Response;
            Assert.NotNull(response);

            Assert.Equal(p1, response._id1);
            Assert.Equal(p2, response._id2);
            Assert.Equal(p3, response._id3);
        }



        [Fact]

        public async void Test2()
        {
            var type = typeof(MultiParameteredPathTestController);

            string p1 = "test1";
            int p2 = 39;
            float p3 = 39.3939f;


            string path = MakePath(type, "async", p1, p2, p3);

            var action = _router.GetAction("get", path);
            Assert.NotNull(action);
            Assert.Equal(type, action.ControllerType);

            var controller = _provider.GetService(action.ControllerType) as MultiParameteredPathTestController;

            Assert.NotNull(controller);
            Assert.Equal(type, controller.GetType());


            var invoker = action.Invoker;

            Assert.True(action.Invoker.IsAsyncMethod);

            var result = await invoker.InvokeReturnAsync(controller, action.MakeParamters(path, null, null));

            Assert.NotNull(result);

            var response = result as MultiParameteredPathTestController.Response;
            Assert.NotNull(response);

            Assert.Equal(p1, response._id1);
            Assert.Equal(p2, response._id2);
            Assert.Equal(p3, response._id3);
        }




    }
}
