using Interfaces;
using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using System;
using System.IO;
using System.Net;
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

            var config = new ClientConfiguration
            {
                DeploymentId = configuration.GetSection("DeploymentId").Value,
                PropagateActivityId = true
            };

            var hostEntry = Dns.GetHostEntryAsync(configuration.GetSection("ServiceName").Value).Result;
            foreach (var item in hostEntry.AddressList)
            {
                Console.WriteLine(item);
            }
            var ip = hostEntry.AddressList[0];
            config.Gateways.Add(new IPEndPoint(ip, Convert.ToInt32(configuration.GetSection("ProxyGatewayPort").Value)));

            var client = new ClientBuilder()
                .AddApplicationPartsFromBasePath()
                .UseConfiguration(config)
                .Build();

            client.Connect().Wait();

            var friend = client.GetGrain<IHelloGrain>(0);

            for (int i = 0; i < 10; i++)
            {
                var response = friend.SayHello("Good morning, my friend" + i).Result;
                Console.WriteLine("\n\n{0}\n\n", response);
                Thread.Sleep(500);
            }

            Console.ReadLine();
        }
    }
}
