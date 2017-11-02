using Grains;
using Orleans.Hosting;
using Orleans.Runtime.Configuration;
using System;
using System.Threading.Tasks;

namespace Silo
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = StartSilo().Result;

            host.Stopped.Wait();
        }

        private static async Task<ISiloHost> StartSilo()
        {
            var config = new ClusterConfiguration();
            config.LoadFromFile("Config/OrleansConfiguration.xml");

            var builder = new SiloHostBuilder()
                .UseConfiguration(config)
                .AddApplicationPartsFromReferences(typeof(HelloGrain).Assembly);

            var host = builder.Build();
            await host.StartAsync();
            Console.WriteLine("Application started. Listening on " + config.Defaults.ProxyGatewayEndpoint);

            return host;
        }
    }
}
