using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Npgg.ApiRouter.Tests
{
    public class CustomAttributeCacheTests
    {
        [Fact]
        public void CustomAttributeCacheTest()
        {
            var value = 3939;
            var action = RouteHelper.GetActions(typeof(TestController)).First();

            var attr = action.GetCustomAttribute<CustomAttribute>();

            Assert.Equal(0, attr.Value);
            attr.Value = value;

            Assert.Equal(value, action.GetCustomAttribute<CustomAttribute>().Value);
        }

        [Fact]
        public void CustomAttributeNotExistTest()
        {
            var action = RouteHelper.GetActions(typeof(TestController)).Last();

            Assert.Null(action.GetCustomAttribute<CustomAttribute>());
        }

        [Controller("api/[controller]")]
        class TestController
        {
            [Custom]
            [HttpGet]
            public int test()
            {
                return 1;
            }

            [HttpGet]
            public int test2()
            {
                return 1;
            }
        }

        class CustomAttribute : Attribute
        {
            public int Value { get; set; }
        }
    }
}
