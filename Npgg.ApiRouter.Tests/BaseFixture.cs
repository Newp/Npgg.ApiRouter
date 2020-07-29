using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Npgg.ApiRouter.Tests
{
    public class BaseFixture
    {

        protected readonly Router router = new Router();
        protected readonly IServiceProvider provider;
        protected readonly Dictionary<string, HttpAction> routerMap;
        public BaseFixture()
        {
            ServiceCollection collection = new ServiceCollection();
            router.Regist(collection, this.GetType().Assembly); //test 에 있는 controller 들만
            this.provider= collection.BuildServiceProvider();

            this.routerMap = provider.GetService<RouterMap>();
        }

        public async Task<object> Get(string path, QueryStringCollection query)
        {
            string method = "get";
            var action = this.router.GetAction(method, path);
            Assert.NotNull(action);

            var result = await action.Invoke(this.provider, method, path, query);

            return result;
        }
    }
}
