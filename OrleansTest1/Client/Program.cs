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
            Console.Title = "Client" ;
            InitializeWithRetries();

            DoClientWork();
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

        private static void DoClientWork()
        {
            var t1 = Task.Factory.StartNew(() =>
            {
                AddCount();
            });
            var t2 = Task.Factory.StartNew(() =>
            {
                AddCount();
            });
            var t3 = Task.Factory.StartNew(() =>
            {
                AddCount();
            });
            Task.WaitAll(t1, t2, t3);
        }

        static void AddCount()
        {
            var test = GrainClient.GrainFactory.GetGrain<ITest>(0);

            Parallel.For(0, 200, (i) =>
            {
                test.AddCount();
            });
        }

        //private static int num = 0;
        //static void NotGrainAddCount()
        //{
        //    System.Threading.Thread.Sleep(100);
        //    num++;
        //    Console.WriteLine(num);
        //}
    }
}
