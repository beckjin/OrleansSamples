using Grains;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.IO;
using System.Net;
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
                .AddJsonFile("appsettings.json", false, true);
            var configuration = builder.Build();

            var siloPort = int.Parse(configuration.GetSection("SiloPort").Value);
            var gatewayPort = int.Parse(configuration.GetSection("GatewayPort").Value);

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
                 .ConfigureEndpoints(Dns.GetHostName(), siloPort, gatewayPort)
                 .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(HelloGrain).Assembly).WithReferences())
                 .ConfigureLogging(log => log.SetMinimumLevel(LogLevel.Warning).AddConsole())
                 .Build();

            Task.Run(StartSilo);

            Console.CancelKeyPress += (sender, e) =>
            {
                Task.Run(StopSilo);
                siloStopped.WaitOne();
            };

            AppDomain.CurrentDomain.ProcessExit += (s, e) =>
            {
                Task.Run(StopSilo);
                siloStopped.WaitOne();
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
            await silo.StopAsync();
            Console.WriteLine("Silo stopped");
            siloStopped.Set();
        }
    }
}
