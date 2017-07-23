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

            var joe = GrainClient.GrainFactory.GetGrain<IPersonGrain>("Joe");
            joe.SayHelloAsync();
            joe.SayHelloAsync();

            var sam = GrainClient.GrainFactory.GetGrain<IPersonGrain>("Sam");
            sam.SayHelloAsync();
            sam.SayHelloAsync();

            Console.ReadLine();
        }
    }
}
