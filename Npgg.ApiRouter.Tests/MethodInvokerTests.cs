using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
namespace Npgg.ApiRouter.Tests
{

    public partial class MethodInvokerTests
    {
        [Fact]
        public void MultiParameteredPathRouteTest()
        {

            var controller = new MultiParameteredPathTestController();
            var list = RouteHelper.GetActions(controller.GetType());


            string id1 = "mikumiku";
            int id2 = 3939;
            float id3 = 39.3939f;

            var invoker = list.First();
            var parameters = invoker.MakeParamters($"get/api/MultiParameteredPath/{id1}/{id2}/{id3}", null, null);


            Assert.Null(controller._id1);
            Assert.Equal(0, controller._id2);
            Assert.Equal(0, controller._id3);



            invoker.Invoker.InvokeReturn(controller, parameters);

            Assert.Equal(id1, controller._id1);
            Assert.Equal(id2, controller._id2);
            Assert.Equal(id3, controller._id3);
        }

        //

        [Fact]
        public void MethodInvokerTest()
        {
            TestCommand cmd = new TestCommand();

            Assert.Equal(0, cmd.value);
            int value = 39;

            var method = cmd.GetType().GetMethod("test");
            MethodInvoker invoker = new MethodInvoker(method);

            invoker.InvokeVoid(cmd, new object[] { value });

            Assert.Equal(value, cmd.value);
        }

        class TestCommand
        {

            public int value;
            public void test(int value)
            {
                this.value = value;
            }
        }
    }
}
