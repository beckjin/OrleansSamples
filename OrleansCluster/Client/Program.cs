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

            while (true)
            {
                Console.WriteLine("Press 'exit' to exit...");
                var str = Console.ReadLine();
                if (str == "exit")
                    break;

                DoWork();
            }

            Console.ReadLine();
        }

        static async void DoWork()
        {
            var joe = GrainClient.GrainFactory.GetGrain<IPersonGrain>("Joe");
            await joe.SayHelloAsync();
        }
    }
}
