using Orleans;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using Interfaces;
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

            var random = new Random();
            while (true)
            {
                Thread.Sleep(1000);
                var grainId = random.Next().ToString();
                var grain = GrainClient.GrainFactory.GetGrain<IPersonGrain>("Test-" + grainId);
                grain.SayHelloAsync();
            }
        }
    }
}
