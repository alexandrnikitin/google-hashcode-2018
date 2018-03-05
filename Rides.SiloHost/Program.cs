using System;
using System.Threading.Tasks;
using Orleans.Runtime.Configuration;

namespace Rides.SiloHost
{
    class Program
    {
        static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                var host = await StartSilo();
                Console.WriteLine("Press Enter to terminate...");
                Console.ReadLine();

                host.ShutdownOrleansSilo();
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }

        private static async Task<Orleans.Runtime.Host.SiloHost> StartSilo()
        {
            // define the cluster configuration
            var siloConfig = ClusterConfiguration.LocalhostPrimarySilo();
            var silo = new Orleans.Runtime.Host.SiloHost("Test Silo", siloConfig);
            silo.InitializeOrleansSilo();
            silo.StartOrleansSilo();
            return silo;
        }
    }
}