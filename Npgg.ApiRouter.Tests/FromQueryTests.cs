using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Npgg.ApiRouter.Tests
{

    public class FromQueryTests : BaseFixture
    {

        public FromQueryTests()
        {
        }

        [Fact]
        public async Task FromQuery1ParameterTest()
        {
            QueryStringCollection query = new QueryStringCollection();

            string queryValue = "yes echo gogo";
            query.AddValue("queryValue", queryValue);

            string path = $"/api/FromQueryTest/1param";

            var result = (string)await base.Get(path, query);

            Assert.StartsWith("echo:", result);
            Assert.EndsWith(queryValue, result);
        }

        [Fact]
        public async Task FromQuery2ParameterTest()
        {
            QueryStringCollection query = new QueryStringCollection();

            string queryValue1 = "yes echo gogo";

            string queryValue2 = "goodgoodverygood";
            query.AddValue("queryValue1", queryValue1);
            query.AddValue("queryValue2", queryValue2);

            string path = $"/api/FromQueryTest/2params";

            var result = await base.Get(path, query);

            var je = JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(result));

            var echo1 = je.GetProperty("echo1").GetString();
            var echo2 = je.GetProperty("echo2").GetString();

            Assert.Equal(queryValue1, echo1);
            Assert.Equal(queryValue2, echo2);
            //Assert.StartsWith("echo:", result);
            //Assert.EndsWith(queryValue, result);
        }
    }
}
