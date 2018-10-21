using Interfaces;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        private static IClusterClient client = null;

        static void Main(string[] args)
        {
            Console.Title = "Client";
            client = InitialiseClient().Result;
            if (client != null)
            {
                while (true)
                {
                    Console.WriteLine("Press 'exit' to exit...");
                    var input = Console.ReadLine();
                    if (input == "exit") break;

                    // 发布消息
                    var publisherGrain = client.GetGrain<IPublisherGrain>(Guid.Empty);
                    publisherGrain.PublishMessageAsync(input);
                }
            }
            else
            {
                Console.WriteLine("Client init failed.");
            }

            Console.ReadLine();
        }

        private static async Task<IClusterClient> InitialiseClient()
        {
            int tryTimes = 10;
            while (tryTimes > 0)
            {
                try
                {
                    client = new ClientBuilder()
                                .UseAdoNetClustering(options =>
                                {
                                    options.Invariant = "MySql.Data.MySqlClient";
                                    options.ConnectionString = "server=localhost;port=3306;database=orleans;user id=root;password=;SslMode=none;";
                                })
                                .Configure<ClusterOptions>(options =>
                                {
                                    options.ClusterId = "dev";
                                    options.ServiceId = "OrleansTest";
                                })
                                .AddSimpleMessageStreamProvider("SMSProvider")
                                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IPublisherGrain).Assembly))
                                .ConfigureLogging(log => log.SetMinimumLevel(LogLevel.Warning).AddConsole())
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
