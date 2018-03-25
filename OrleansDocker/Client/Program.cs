using Interfaces;
using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("Config/appsettings.json", false, true);
            var configuration = builder.Build();

            var client = new ClientBuilder()
               .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IHelloGrain).Assembly))
               .Configure<ClusterOptions>(options =>  options.ClusterId = configuration.GetSection("ClusterId").Value)
               .UseAdoNetClustering(options =>
               {
                   options.Invariant = "System.Data.SqlClient";
                   options.ConnectionString = configuration.GetSection("ConnectionString").Value;
               })
               .Build();

            StartClientWithRetries(client).Wait();

            var friend = client.GetGrain<IHelloGrain>(0);

            for (int i = 0; i < 10; i++)
            {
                var response = friend.SayHello("Good morning, my friend" + i).Result;
                Console.WriteLine("\n{0}\n", response);
                Thread.Sleep(500);
            }

            Console.ReadLine();
        }

        private static async Task StartClientWithRetries(IClusterClient client)
        {
            for (var i = 0; i < 5; i++)
            {
                try
                {
                    await client.Connect();
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }
    }
}
