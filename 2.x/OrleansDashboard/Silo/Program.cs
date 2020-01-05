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
        private const string ClusterId = "dev";
        private const string ServiceId = "OrleansSample";

        private const string Invariant = "MySql.Data.MySqlClient";
        private const string ConnectionString = "server=localhost;port=3306;database=orleans;user id=root;password=;SslMode=none;";

        static void Main(string[] args)
        {
            Console.WriteLine("输入Silo序号:");
            var index = Convert.ToInt32(Console.ReadLine());
            Console.Title = "Silo" + index;

            RunMainAsync(11111 + index, 30000 + index, 2233 + index).Wait();

            Console.ReadKey();
        }

        private static async Task RunMainAsync(int siloPort, int gatewayPort, int dashboardPort)
        {
            try
            {
                var host = await InitialiseSilo(siloPort, gatewayPort, dashboardPort);
                Console.WriteLine("Silo started successfully");
                Console.WriteLine("Press enter to exit...");
                Console.ReadLine();
                await host.StopAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static async Task<ISiloHost> InitialiseSilo(int siloPort, int gatewayPort, int dashboardPort)
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
                .UseDashboard(options =>
                {
                    options.Username = "admin";
                    options.Password = "123456";
                    options.Port = dashboardPort;

                })
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = ClusterId;
                    options.ServiceId = ServiceId;
                })
                .ConfigureEndpoints(siloPort: siloPort, gatewayPort: gatewayPort)
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(PersonGrain).Assembly).WithReferences())
                .ConfigureLogging(log => log.SetMinimumLevel(LogLevel.Warning).AddConsole());

            var host = builder.Build();
            await host.StartAsync();

            return host;
        }
    }
}
