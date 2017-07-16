using Orleans;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Client
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
                DoClientWork();
            }

            Console.ReadLine();
        }

        private static async Task DoClientWork()
        {
            var grain = GrainClient.GrainFactory.GetGrain<IStockGrain>("MSFT");
            await grain.GetPrice();
        }
    }
}
