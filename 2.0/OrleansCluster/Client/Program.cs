using Interfaces;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Threading;
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
                var random = new Random();
                while (true)
                {
                    Thread.Sleep(1000);
                    var grainId = random.Next().ToString();
                    var grain = client.GetGrain<IPersonGrain>("Test------" + grainId);
                    grain.SayHelloAsync();
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
                                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IPersonGrain).Assembly))
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
