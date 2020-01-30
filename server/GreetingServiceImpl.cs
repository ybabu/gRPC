﻿using Greet;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Greet.GreetingService;

namespace server
{
   public class GreetingServiceImpl: GreetingServiceBase
    {
        public override Task<GreetingResponse> Greet(GreetingRequest request, ServerCallContext context)
        {
            string result = String.Format("Hello {0} {1}",request.Greeting.FirstName ,request.Greeting.LatsName );
            return Task.FromResult(new GreetingResponse() { Result = result });
        }
        public override  async Task GreetingManyTimes(GreetingManyTimesRequest request, IServerStreamWriter<GreetingManyTimesResponse> responseStream, ServerCallContext context)
        {
            Console.WriteLine("The Server recieved the equest:");
            Console.WriteLine(request.ToString());
            string result = String.Format("Hello {0} {1}", request.Greeting.FirstName, request.Greeting.LatsName);
            foreach (int i in Enumerable.Range(1, 10))
            {
                await responseStream.WriteAsync(new GreetingManyTimesResponse() { Result = result });
            }
            
        }

    }
}
