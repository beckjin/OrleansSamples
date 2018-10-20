using Grains;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Silo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Silo";
            StartSilo().Wait();
        }

        private static async Task StartSilo()
        {
            var builder = new SiloHostBuilder()
                //.AddMemoryGrainStorage("OrleansStorage", options => options.NumStorageGrains = 10)
                .AddAdoNetGrainStorage("OrleansStorage", options =>
                {
                    options.Invariant = "MySql.Data.MySqlClient";
                    options.ConnectionString = "server=localhost;port=3306;database=orleans;user id=root;password=;SslMode=none;";
                    options.UseJsonFormat = true;
                })
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "OrleansTest";
                })
                .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(PersonGrain).Assembly).WithReferences())
                .ConfigureLogging(log => log.SetMinimumLevel(LogLevel.Warning).AddConsole());

            var host = builder.Build();
            await host.StartAsync();

            Console.WriteLine("Silo started successfully");
            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
            await host.StopAsync();
        }
    }
}
