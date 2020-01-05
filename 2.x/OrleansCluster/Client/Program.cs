using Interfaces;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Runtime;
using System;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        private const string ClusterId = "dev";
        private const string ServiceId = "OrleansSample";

        private const string Invariant = "MySql.Data.MySqlClient";
        private const string ConnectionString = "server=localhost;port=3306;database=orleans;user id=root;password=;SslMode=none;";

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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static async Task<IClusterClient> InitialiseClient()
        {
            var client = new ClientBuilder()
                    .UseAdoNetClustering(options =>
                    {
                        options.Invariant = Invariant;
                        options.ConnectionString = ConnectionString;
                    })
                    .Configure<ClusterOptions>(options =>
                    {
                        options.ClusterId = ClusterId;
                        options.ServiceId = ServiceId;
                    })
                    .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IPersonGrain).Assembly))
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
            var random = new Random();
            while (true)
            {
                await Task.Delay(1000);
                var grainId = random.Next().ToString();
                var grain = client.GetGrain<IPersonGrain>("Test------" + grainId);
                await grain.SayHelloAsync();
            }
        }
    }
}
