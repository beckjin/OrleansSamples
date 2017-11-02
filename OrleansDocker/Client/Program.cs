using Interfaces;
using Orleans;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using System;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var client = StartClientWithRetries().Result;

                DoClientWork(client).Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadLine();
        }

        private static async Task<IClusterClient> StartClientWithRetries(int initializeAttemptsBeforeFailing = 5)
        {
            int attempt = 0;
            IClusterClient client;
            while (true)
            {
                try
                {
                    var config = ClientConfiguration.LoadFromFile("Config/ClientConfiguration.xml");

                    client = new ClientBuilder()
                        .UseConfiguration(config)
                        .AddApplicationPartsFromReferences(typeof(IHelloGrain).Assembly)
                        .Build();

                    await client.Connect();
                    Console.WriteLine("Client successfully connect to silo host");
                    break;
                }
                catch (SiloUnavailableException)
                {
                    attempt++;
                    Console.WriteLine($"Attempt {attempt} of {initializeAttemptsBeforeFailing} failed to initialize the Orleans client.");
                    if (attempt > initializeAttemptsBeforeFailing)
                    {
                        throw;
                    }
                    await Task.Delay(TimeSpan.FromSeconds(4));
                }
            }

            return client;
        }

        private static async Task DoClientWork(IClusterClient client)
        {
            var friend = client.GetGrain<IHelloGrain>(0);
            var response = await friend.SayHello("Good morning, my friend!");
            Console.WriteLine("\n\n{0}\n\n", response);
        }
    }
}
