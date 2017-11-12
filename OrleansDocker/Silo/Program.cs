using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Orleans.Hosting;
using Orleans.Runtime.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace Silo
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Config/appsettings.json", false, true);
            var configuration = builder.Build();

            var config = new ClusterConfiguration();
            config.Globals.DataConnectionString = configuration.GetSection("DataConnectionString").Value;
            config.Globals.DeploymentId = configuration.GetSection("DeploymentId").Value;
            config.Globals.LivenessType = GlobalConfiguration.LivenessProviderType.SqlServer;
            config.Globals.ReminderServiceType = GlobalConfiguration.ReminderServiceProviderType.SqlServer;

            config.Defaults.PropagateActivityId = true;
            config.Defaults.ProxyGatewayEndpoint = new IPEndPoint(IPAddress.Any, Convert.ToInt32(configuration.GetSection("ProxyGatewayPort").Value));
            config.Defaults.Port = Convert.ToInt32(configuration.GetSection("Port").Value);
            var ips = Dns.GetHostAddressesAsync(Dns.GetHostName()).Result;
            config.Defaults.HostNameOrIPAddress = ips.FirstOrDefault()?.ToString();

            var silo = new SiloHostBuilder()
                 .AddApplicationPartsFromAppDomain()
                 .AddApplicationPartsFromBasePath()
                 .UseConfiguration(config)
                 .ConfigureLogging(configure => configure.SetMinimumLevel(LogLevel.Warning).AddConsole())
                 .Build();

            silo.StartAsync().Wait();

            Console.WriteLine("Application started.");

            silo.Stopped.Wait();
        }
    }
}
