using Interfaces;
using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.Hosting;
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

            var client = new ClientBuilder()
               .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IHelloGrain).Assembly))
               .ConfigureCluster(options =>  options.ClusterId = configuration.GetSection("ClusterId").Value)
               .UseAdoNetClustering(options =>
               {
                   options.AdoInvariant = "System.Data.SqlClient";
                   options.ConnectionString = configuration.GetSection("ConnectionString").Value;
               })
               .Build();

            client.Connect().Wait();

            var friend = client.GetGrain<IHelloGrain>(0);

            for (int i = 0; i < 10; i++)
            {
                var response = friend.SayHello("Good morning, my friend" + i).Result;
                Console.WriteLine("\n{0}\n", response);
                Thread.Sleep(500);
            }

            Console.ReadLine();
        }
    }
}
