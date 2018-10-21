using Grains;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Threading.Tasks;

namespace Silo
{
    class Program
    {
        private const string Invariant = "MySql.Data.MySqlClient";
        private const string ConnectionString = "server=localhost;port=3306;database=orleans;user id=root;password=;SslMode=none;";

        static void Main(string[] args)
        {
            Console.Title = "Silo";
            StartSilo().Wait();
        }

        private static async Task StartSilo()
        {
            var builder = new SiloHostBuilder()
                // Grain State
                .AddAdoNetGrainStorage("PubSubStore", options =>
                {
                    options.Invariant = Invariant;
                    options.ConnectionString = ConnectionString;
                    options.UseJsonFormat = true;
                })
                // Membership
                .UseAdoNetClustering(options =>
                {
                    options.Invariant = Invariant;
                    options.ConnectionString = ConnectionString;
                })
                .AddSimpleMessageStreamProvider("SMSProvider")
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "OrleansTest";
                })
                .ConfigureEndpoints(siloPort: 11111, gatewayPort: 30000)
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(PublisherGrain).Assembly).WithReferences())
                .ConfigureLogging(log => log.SetMinimumLevel(LogLevel.Warning).AddConsole());

            var host = builder.Build();
            await host.StartAsync();

            Console.WriteLine("Silo started successfully");
            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
            Console.WriteLine("-------------------------------");
            await host.StopAsync();
        }
    }
}
