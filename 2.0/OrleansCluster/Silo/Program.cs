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
            Console.WriteLine("输入Silo序号:");
            var index = Convert.ToInt32(Console.ReadLine());

            Console.Title = "Silo" + index;
            StartSilo(11111 + index, 30000 + index).Wait();
        }

        private static async Task StartSilo(int siloPort, int gatewayPort)
        {
            var builder = new SiloHostBuilder()
                // Grain State
                .AddAdoNetGrainStorage("OrleansStorage", options =>
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
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "OrleansTest";
                })
                .ConfigureEndpoints(siloPort: siloPort, gatewayPort: gatewayPort)
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(PersonGrain).Assembly).WithReferences())
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
