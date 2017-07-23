using Interfaces;
using Orleans;
using Orleans.Runtime;
using System;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Please input ConfigFileName number:");
            var number = Console.ReadLine();

            Console.Title = "Client" + number;
            InitializeWithRetries(number);

            DoClientWork().Wait();

            Console.WriteLine("Press any key to stop.");
            Console.ReadLine();
        }

        private static void InitializeWithRetries(string number)
        {
            try
            {
                GrainClient.Initialize("ClientConfiguration" + number + ".xml");
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
            var friend = GrainClient.GrainFactory.GetGrain<IHello>(0);

            Console.WriteLine("{0}", await friend.SayHello("Hi,a"));
            Console.WriteLine("{0}", await friend.SayHello("Hi,b"));
            Console.WriteLine("{0}", await friend.SayHello("Hi,c"));
            Console.WriteLine("{0}", await friend.SayHello("Hi,d"));
        }
    }
}
