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
        //Unary---Uncomment for Unary 
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

        //    //Server Stream
        //static async Task Main(string[] args)
        //{
        //    Channel channel = new Channel(target, ChannelCredentials.Insecure);
        //   await channel.ConnectAsync().ContinueWith((task) =>
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
        //    var request = new GreetingManyTimesRequest() { Greeting = greeting };
        //    var response = client.GreetingManyTimes(request);

        //    while(await response.ResponseStream.MoveNext())
        //    {
        //        Console.WriteLine(response.ResponseStream.Current.Result);
        //        await Task.Delay(200);
        //    }
        //    channel.ShutdownAsync().Wait();
        //    Console.ReadKey();
        //}

        //Client Stream
        static async Task Main(string[] args)
        {
            Channel channel = new Channel(target, ChannelCredentials.Insecure);
            await channel.ConnectAsync().ContinueWith((task) =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                    Console.WriteLine("The Client connected successfully");
            });
         
            var client = new GreetingService.GreetingServiceClient(channel);
            var greeting = new Greeting()
            {
                FirstName = "Yogi",
                LatsName = "Babu"
            };
            var request = new LongGreetRequest() { Greeting = greeting };
            var stream = client.LongGreet();
            foreach (int i in Enumerable.Range(1, 10))
            {
                await stream.RequestStream.WriteAsync(request);
            }
            await stream.RequestStream.CompleteAsync();//request call complete then response

            var response = await stream.ResponseAsync;
            Console.WriteLine(response.Result);


            channel.ShutdownAsync().Wait();
            Console.ReadKey();
        }
    }
}
