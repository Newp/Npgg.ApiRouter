using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Npgg.ApiRouter.Tests.TestController
{
    [Controller("api/[controller]")]
    class AllInvokeController
    {

        [HttpGet("Async/{input}")]
        public async Task<string> Async(string input)
        {
            await Task.CompletedTask;
            return "echo:" + input;
        }

        [HttpGet("Sync/{input}")]
        public string Sync(string input)
        {
            return "echo:" + input;
        }




        [HttpGet("AsyncParameterless")]
        public async Task<string> AsyncParameterless()
        {
            await Task.CompletedTask;
            return "AsyncParameterless";
        }

        [HttpGet("SyncParameterless")]
        public string SyncParameterless()
        {
            return "SyncParameterless";
        }


        public static string LastInvoked = null;


        [HttpGet("AsyncParameterlessVoid")]
        public async void AsyncParameterlessVoid()
        {
            LastInvoked = "AsyncParameterlessVoid";
            await Task.CompletedTask;
        }
        [HttpGet("SyncParameterlessVoid")]
        public void SyncParameterlessVoid()
        {
            LastInvoked = "SyncParameterlessVoid";
        }


        [HttpGet("SyncVoid/{input}")]
        public void SyncVoid(string input)
        {
            LastInvoked = input;
        }

        [HttpGet("AsyncVoid/{input}")]
        public async void AsyncVoid(string input)
        {
            LastInvoked = input;
            await Task.CompletedTask;
        }


    }

}
