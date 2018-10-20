using Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
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
                var t1 = Task.Factory.StartNew(() =>
                {
                    AddCount("T1");
                });
                var t2 = Task.Factory.StartNew(() =>
                {
                    AddCount("T2");
                });
                var t3 = Task.Factory.StartNew(() =>
                {
                    AddCount("T3");
                });
                Task.WaitAll(t1, t2, t3);
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
                                .UseLocalhostClustering()
                                .Configure<ClusterOptions>(options =>
                                {
                                    options.ClusterId = "dev";
                                    options.ServiceId = "OrleansTest";
                                })
                                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(ITestGrain).Assembly))
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


        static void AddCount(string taskName)
        {
            var test = client.GetGrain<ITestGrain>(0);

            for (int i = 0; i < 200; i++)
            {
                test.AddCount(taskName);
            }
        }
    }
}