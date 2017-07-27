using Interfaces;
using Orleans;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using Orleans.Streams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            var streamProvider = GrainClient.GetStreamProvider("SMSProvider");
            var stream = streamProvider.GetStream<int>(Guid.Empty, "RANDOMDATA");
            await stream.SubscribeAsync((data, token) =>
            {
                Console.WriteLine($"Received data: {data}");
                return Task.CompletedTask;
            });
        }
    }
}
