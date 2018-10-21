using Interfaces;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Runtime;
using System;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        private const int InitializeAttemptsBeforeFailing = 5;
        private static int attempt = 0;

        static void Main(string[] args)
        {
            Console.Title = "Client";

            RunMainAsync().Wait();

            Console.ReadKey();
        }

        private static async Task RunMainAsync()
        {
            try
            {
                using (var client = await InitialiseClient())
                {
                    Console.WriteLine($"开始执行....");
                    await DoClientWork(client);
                    Console.WriteLine($"执行结束");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
            }
        }

        private static async Task<IClusterClient> InitialiseClient()
        {
            var client = new ClientBuilder()
                        .UseLocalhostClustering()
                        .Configure<ClusterOptions>(options =>
                        {
                            options.ClusterId = "dev";
                            options.ServiceId = "OrleansTest";
                        })
                        .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(ITestGrain).Assembly))
                        .ConfigureLogging(log => log.SetMinimumLevel(LogLevel.Warning).AddConsole())
                        .Build();

            await client.Connect(RetryFilter);
            return client;
        }

        private static async Task<bool> RetryFilter(Exception exception)
        {
            if (exception.GetType() != typeof(SiloUnavailableException))
            {
                Console.WriteLine($"Cluster client failed to connect to cluster with unexpected error.  Exception: {exception}");
                return false;
            }
            attempt++;
            Console.WriteLine($"Cluster client attempt {attempt} of {InitializeAttemptsBeforeFailing} failed to connect to cluster.  Exception: {exception}");
            if (attempt > InitializeAttemptsBeforeFailing)
            {
                return false;
            }
            await Task.Delay(TimeSpan.FromSeconds(3));
            return true;
        }

        private static async Task DoClientWork(IClusterClient client)
        {
            var grain = client.GetGrain<ITestGrain>(0);
            var t1 = AddCount(grain, "T1");
            var t2 = AddCount(grain, "T2");
            var t3 = AddCount(grain, "T3");
            Task.WaitAll(t1, t2, t3);
            await Task.CompletedTask;
        }

        private static async Task AddCount(ITestGrain grain, string taskName)
        {
            for (int i = 0; i < 200; i++)
            {
               await grain.AddCount(taskName);
            }
        }
    }
}