using System;
using System.Collections.Generic;
using System.Text;

namespace Npgg.ApiRouter.Tests.TestController
{

    [Controller("api/[controller]")]
    class FromQueryTestController
    {
        [HttpGet("1param")]
        public string Get([FromQuery] string queryValue)
        {
            return "echo:" + queryValue;
        }

        [HttpGet("2params")]
        public dynamic Get([FromQuery] string queryValue1, [FromQuery] string queryValue2)
        {
            return new
            {
                echo1 = queryValue1,
                echo2 = queryValue2
            };
        }

    }
}
