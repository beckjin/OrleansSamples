using Grains;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace Silo
{
    class Program
    {
        private static ISiloHost silo;
        private static readonly ManualResetEvent siloStopped = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Config/appsettings.json", false, true);
            var configuration = builder.Build();

            // 以太网 IPv4
            var nodeIps = NetworkInterface.GetAllNetworkInterfaces()
                .Where(iface => iface.NetworkInterfaceType == NetworkInterfaceType.Ethernet).SelectMany(iface => iface.GetIPProperties().UnicastAddresses)
                .Where(addr => addr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).Select(addr => addr.Address)
                .ToList();

            IPAddress address = nodeIps[0];

            silo = new SiloHostBuilder()
                 .Configure<ClusterOptions>(options =>
                 {
                     options.ClusterId = configuration.GetSection("ClusterId").Value;
                     options.ServiceId = configuration.GetSection("ServiceId").Value;
                 })
                 .UseAdoNetClustering(options =>
                 {
                     options.Invariant = configuration.GetSection("Invariant").Value;
                     options.ConnectionString = configuration.GetSection("ConnectionString").Value;

                 })
                 .ConfigureEndpoints(address, 11111, 30000)
                 .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(HelloGrain).Assembly).WithReferences())
                 .ConfigureLogging(log => log.SetMinimumLevel(LogLevel.Warning).AddConsole())
                 .Build();

            Task.Run(StartSilo);

            Console.CancelKeyPress += (sender, e) =>
            {
                Task.Run(StopSilo);
            };

            AppDomain.CurrentDomain.ProcessExit += (s, e) =>
            {
                Task.Run(StopSilo);
            };

            siloStopped.WaitOne();
        }


        private static async Task StartSilo()
        {
            await silo.StartAsync();
            Console.WriteLine("Silo started");
        }

        private static async Task StopSilo()
        {
            if (silo != null)
            {
                await silo.StopAsync();
                Console.WriteLine("Silo stopped");
            }
            siloStopped.Set();
        }
    }
}
