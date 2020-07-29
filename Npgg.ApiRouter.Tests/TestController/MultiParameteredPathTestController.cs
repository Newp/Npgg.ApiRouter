using System.Threading.Tasks;

namespace Npgg.ApiRouter.Tests
{

    [Controller("api/[controller]")]
    public class MultiParameteredPathTestController
    {
        public string _id1;
        public int _id2;
        public float _id3;


        [HttpGet("{id1}/{id2}/{id3}")]
        public Response TestAction(string id1, int id2, float id3)
        {
            this._id1 = id1;
            this._id2 = id2;
            this._id3 = id3;

            var result = new Response()
            {
                _id1 = id1,
                _id2 = id2,
                _id3 = id3,
            };

            return result;
        }


        [HttpGet("async/{id1}/{id2}/{id3}")]
        public async Task<Response> TestActionAsync(string id1, int id2, float id3)
        {
            this._id1 = id1;
            this._id2 = id2;
            this._id3 = id3;

            var result = new Response()
            {
                _id1 = id1,
                _id2 = id2,
                _id3 = id3,
            };
            
            return await Task.FromResult(result);
        }

        public class Response
        {
            public string _id1;
            public int _id2;
            public float _id3;

        }
    }

}
