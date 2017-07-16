using Orleans;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using State.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace State.Client
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

            Console.ReadLine();
        }
    }
}
