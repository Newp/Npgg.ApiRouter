using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Npgg.ApiRouter.Tests
{
    public class RoutePathNameTester
    {
        [Fact]
        public void Test()
        {
            var action = RouteHelper.GetActions(typeof(TestController)).First();

            
        }

        [Controller("api/[controller]")]
        class TestController
        {
            [HttpGet]
            public int test()
            {
                return 1;
            }
        }
    }
}
