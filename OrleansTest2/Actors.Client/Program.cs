using Actors.Interfaces;
using Orleans;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actors.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            InitializeWithRetries();

            DoClientWork().Wait();

            Console.WriteLine("Press any key to stop.");
            Console.ReadLine();
        }

        private static void InitializeWithRetries()
        {
            try
            {
                GrainClient.Initialize("ClientConfiguration.xml");
                Console.WriteLine("Client successfully connect to silo host.");
            }
            catch (SiloUnavailableException ex)
            {
                Console.WriteLine($"Failed to initialize the Orleans client.\n\n{ex}");
                Console.ReadLine();
            }
        }

        private static async Task DoClientWork()
        {
            var grainFactory = GrainClient.GrainFactory;
            var e0 = grainFactory.GetGrain<IEmployee>(Guid.NewGuid());
            var m1 = grainFactory.GetGrain<IManager>(Guid.NewGuid());
            await m1.AddDirectReport(e0);

        }
    }
}
