using Interfaces;
using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.IO;
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

            var client = InitialiseClient(configuration).Result;

            if (client != null)
            {
                var friend = client.GetGrain<IHelloGrain>(0);

                for (int i = 0; i < 10; i++)
                {
                    var response = friend.SayHello("Hi," + i).Result;
                    Console.WriteLine("\n{0}", response);
                    Thread.Sleep(500);
                }
            }
            else
            {
                Console.WriteLine("Client init failed.");
            }

            Console.ReadLine();
        }

        private static async Task<IClusterClient> InitialiseClient(IConfigurationRoot configuration)
        {
            IClusterClient client = null;

            int tryTimes = 10;
            while (tryTimes > 0)
            {
                try
                {
                    client = new ClientBuilder()
                              .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IHelloGrain).Assembly))
                              .Configure<ClusterOptions>(options =>
                              {
                                  options.ClusterId = configuration.GetSection("ClusterId").Value;
                                  options.ServiceId = configuration.GetSection("ServiceId").Value;
                              })
                              .UseAdoNetClustering(options =>
                              {
                                  options.Invariant = "System.Data.SqlClient";
                                  options.ConnectionString = configuration.GetSection("ConnectionString").Value;
                              })
                              .Build();

                    await client.Connect();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    await Task.Delay(TimeSpan.FromSeconds(5));

                    if (client != null && !client.IsInitialized)
                    {
                        client.Dispose();
                        client = null;
                    }
                }
                tryTimes--;
            }

            return client;
        }
    }
}
