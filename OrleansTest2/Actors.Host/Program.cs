using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actors.Host
{
    class Program
    {
        private static OrleansHostWrapper hostWrapper;

        static void Main(string[] args)
        {
            InitSilo(args);

            Console.WriteLine("Press Enter to stop.");
            Console.ReadLine();

            ShutdownSilo();
        }

        static void InitSilo(string[] args)
        {
            hostWrapper = new OrleansHostWrapper(args);

            hostWrapper.Run();
        }

        static void ShutdownSilo()
        {
            if (hostWrapper != null)
            {
                hostWrapper.Dispose();
                GC.SuppressFinalize(hostWrapper);
            }
        }
    }
}
