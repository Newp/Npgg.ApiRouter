using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Npgg.ApiRouter.Tests
{
    public class QueryStringCollectionTests
    {


        [Fact] public void IntTest() => AddGetCheck(3939500);
        [Fact] public void StringTest() => AddGetCheck("hihi stiring test");
        [Fact] public void FloatTest() => AddGetCheck(3.14f);
        [Fact] public void DoubleTest() => AddGetCheck(3.14);
        [Fact] public void UintTest() => AddGetCheck(500000u);
        [Fact] public void LongTest() => AddGetCheck(500000l);

        void AddGetCheck<T>(T value)
        {

            string key = "key_" + new Random().Next(50000);
            QueryStringCollection collection = new QueryStringCollection();
            collection.AddValue(key, value);

            Assert.True(collection.TryGetFirst<T>(key, out var result));
            Assert.Equal(value, result);
        }

        [Fact]
        public void DateTimeTest()
        {

            string key = "key_" + new Random().Next(50000);
            QueryStringCollection collection = new QueryStringCollection();
            DateTime time = DateTime.Now;
            collection.AddValue(key, time.ToString("o"));

            Assert.True(collection.TryGetFirst<DateTime>(key, out var result));
            Assert.Equal(time, result);
        }

        [Fact]
        public void DeserializeTest()
        {
            var json = File.ReadAllText("SampleJson/gateway_querystring.json");
            var collection = JsonSerializer.Deserialize<QueryStringCollection>(json);
            Assert.Equal(2, collection.Count);

            Assert.True(collection.TryGetFirst<string>("needs", out var result));
            Assert.Equal("miku", result);

            var needs = collection.GetValues<string>("needs");


            Assert.Equal(3, needs.Count);
        }

        [Fact]
        public void ToStringTest()
        {
            var query = new QueryStringCollection();

            var list = Enumerable.Range(1, 5).ToArray();
            foreach (var i in list)
            {
                query.AddValue("a", i);
            }

            Assert.Equal("a=1&a=2&a=3&a=4&a=5", query.ToString());
        }



        [Fact]
        public void ToStringTest2()
        {
            var query = new QueryStringCollection();

            var list = Enumerable.Range(1, 5).ToArray();
            foreach (var i in list)
            {
                query.AddValue("a", i);
                query.AddValue("b", i);
            }

            
            Assert.Equal("a=1&a=2&a=3&a=4&a=5&b=1&b=2&b=3&b=4&b=5", query.ToString());
        }
    }
}
