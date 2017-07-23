using Orleans.Runtime.Host;
using System;

namespace Silo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please input number:");
            var number = Console.ReadLine();

            Console.Title = "Silo" + number;
            try
            {
                using (var siloHost = new SiloHost(Console.Title))
                {
                    siloHost.ConfigFileName = "OrleansConfiguration" + number + ".xml"; ;
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
