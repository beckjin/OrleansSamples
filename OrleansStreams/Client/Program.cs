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

            Subscribe();

            Console.ReadLine();
        }

        static async void Subscribe()
        {
            var guid = Guid.NewGuid();
            Console.Write(guid);
            var subscriberGrain = GrainClient.GrainFactory.GetGrain<ISubscriberGrain>(guid);
            await subscriberGrain.SubscribeAsync();
        }
    }
}
