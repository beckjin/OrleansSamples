using Interfaces;
using Orleans;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using System;
using System.Threading;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Client";
            var config = ClientConfiguration.LoadFromFile("ClientConfiguration.xml");

            while (true)
            {
                try
                {
                    GrainClient.Initialize(config);
                    Console.WriteLine("Connected to silo");
                    break;
                }
                catch (SiloUnavailableException)
                {
                    Console.WriteLine("Silo not available! Retrying in 3 seconds.");
                    Thread.Sleep(3000);
                }
            }

            //ImplicitPublisher();

            ExplicitSubscriber();

            Console.ReadLine();
        }


        static void ImplicitPublisher()
        {
            while (true)
            {
                Console.WriteLine("Press 'exit' to exit...");
                var input = Console.ReadLine();
                if (input == "exit") break;
                var publisherGrain = GrainClient.GrainFactory.GetGrain<IPublisherGrain>(Guid.NewGuid());
                publisherGrain.PublishMessageAsync(input);
            }
        }

        static void ExplicitSubscriber()
        {
            var guid = Guid.NewGuid();
            var subscriberGrain = GrainClient.GrainFactory.GetGrain<IExplicitSubscriberGrain>(Guid.Empty);
            var streamHandle = subscriberGrain.SubscribeAsync().Result;
            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
            streamHandle.UnsubscribeAsync();
        }
    }
}
