using Dummy;
using Greet;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client
{
    class Program
    {
        const string target = "127.0.0.1:50051";
        //static void Main(string[] args)
        //{
        //    Channel channel = new Channel(target, ChannelCredentials.Insecure);
        //    channel.ConnectAsync().ContinueWith((task)=>
        //    {
        //        if (task.Status == TaskStatus.RanToCompletion)
        //            Console.WriteLine("The Client connected successfully");
        //    });
        //    //;var client = new DummyService.DummyServiceClient(channel);
        //    var client = new GreetingService.GreetingServiceClient(channel);
        //    var greeting = new Greeting()
        //    {
        //        FirstName = "Yogi",
        //        LatsName = "Babu"
        //    };
        //    var request = new GreetingRequest() { Greeting = greeting };
        //    var response = client.Greet(request);
        //    Console.WriteLine(response.Result);

        //    channel.ShutdownAsync().Wait();
        //    Console.ReadKey();
        //}
        static async Task Main(string[] args)
        {
            Channel channel = new Channel(target, ChannelCredentials.Insecure);
           await channel.ConnectAsync().ContinueWith((task) =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                    Console.WriteLine("The Client connected successfully");
            });
            //;var client = new DummyService.DummyServiceClient(channel);
            var client = new GreetingService.GreetingServiceClient(channel);
            var greeting = new Greeting()
            {
                FirstName = "Yogi",
                LatsName = "Babu"
            };
            var request = new GreetingManyTimesRequest() { Greeting = greeting };
            var response = client.GreetingManyTimes(request);

            while(await response.ResponseStream.MoveNext())
            {
                Console.WriteLine(response.ResponseStream.Current.Result);
                await Task.Delay(200);
            }
            channel.ShutdownAsync().Wait();
            Console.ReadKey();
        }
    }
}
