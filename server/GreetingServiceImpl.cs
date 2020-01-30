using Greet;
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
        //Unary 
        public override Task<GreetingResponse> Greet(GreetingRequest request, ServerCallContext context)
        {
            string result = String.Format("Hello {0} {1}",request.Greeting.FirstName ,request.Greeting.LatsName );
            return Task.FromResult(new GreetingResponse() { Result = result });
        }
        //Server Streaming
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
        //Client Streaming
        public override async Task<LongGreetResponse> LongGreet(IAsyncStreamReader<LongGreetRequest> requestStream, ServerCallContext context)
        {
            string result = "";
            while (await requestStream.MoveNext())
            {
                result += string.Format("Hello {0} {1} {2}", requestStream.Current.Greeting.FirstName, requestStream.Current.Greeting.LatsName, Environment.NewLine);
            }

            return new LongGreetResponse() { Result = result };
        }
        // Bi-Directional 
        public override async  Task GreetitEveryone(IAsyncStreamReader<GreetEveryoneRequest> requestStream, IServerStreamWriter<GreetEveryoneResponse> responseStream, ServerCallContext context)
        {

             while (await requestStream.MoveNext() )
            {
                var result = string.Format("Hello {0} {1}",
                                      requestStream.Current.Greeting.FirstName,
                                      requestStream.Current.Greeting.LatsName);
                Console.WriteLine("Recieved :"+result);
                await responseStream.WriteAsync(new GreetEveryoneResponse() { Result = result });

            }
        }


    }
}
