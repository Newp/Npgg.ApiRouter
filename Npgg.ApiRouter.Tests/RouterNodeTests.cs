using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Npgg.ApiRouter.Tests
{
    public class RouterNodeTests
    {
        public RouterNodeTests()
        {
            string[] patterns = new string[]
            {
                "api/values/{id}",
                "api/values/ghost",
                "api/values/ghost/{id}",
                "api/values/all",
            };

            foreach(var pattern in patterns)
            {
                node.Add(pattern.Split('/'), 0, pattern);
            }
        }


        RouterNode<string> node = new RouterNode<string>(-1, "","");

        [Fact]
        public void Test1()
        {
            string path = "api/values";
            node.TryGet(path.Split('/'), 0, out var result);

            Assert.Equal(3, result.childrun.Count);
            Assert.Null(result.Value);
        }

        [Fact]
        public void Test3()
        {
            string path = "api/values/ghost";
            node.TryGet(path.Split('/'), 0, out var result);

            Assert.Equal(path, result.fullPath);
            Assert.Single(result.childrun);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public void Test2()
        {
            string path = "api/values/3";

            Assert.True(node.TryGet(path.Split('/'), 0, out var result));

            Assert.Equal("api/values/{id}", result.fullPath);
            Assert.Empty(result.childrun);
            Assert.NotNull(result.Value);
        }
    }
}
