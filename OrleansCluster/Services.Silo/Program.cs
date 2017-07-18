using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Silo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please input SiloName:");
            var siloName = Console.ReadLine();

            Console.WriteLine("Please input ConfigFileName:");
            var configFileName = Console.ReadLine();

            siloName = string.IsNullOrEmpty(siloName) ? "silo" : siloName;
            configFileName = string.IsNullOrEmpty(configFileName) ? "OrleansConfiguration.xml" : configFileName;

            Console.Title = siloName;
            try
            {
                using (var siloHost = new SiloHost(siloName))
                {
                    siloHost.ConfigFileName = configFileName;
                    siloHost.LoadOrleansConfig();

                    //siloHost.Config.Globals.LivenessType = GlobalConfiguration.LivenessProviderType.Custom;
                    //siloHost.Config.Globals.MembershipTableAssembly = "OrleansConsulUtils";
                    //siloHost.Config.Globals.ReminderServiceType = GlobalConfiguration.ReminderServiceProviderType.Disabled;

                    siloHost.InitializeOrleansSilo();
                    var startedOk = siloHost.StartOrleansSilo();
                    Console.WriteLine("Silo started successfully");

                    Console.WriteLine("Press enter to exit...");
                    Console.ReadLine();
                    siloHost.ShutdownOrleansSilo();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.ReadLine();
        }
    }
}
