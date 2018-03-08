using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using Orleans;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;
using Rides.GrainInterfaces;
using Rides.Grains;

namespace SiloHost1
{
    /// <summary>
    /// Orleans test silo host
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
            // First, configure and start a local silo
            var siloConfig = ClusterConfiguration.LocalhostPrimarySilo();
            siloConfig.Globals.ResponseTimeout = TimeSpan.FromMinutes(10);
            var silo = new SiloHost("TestSilo", siloConfig);
            silo.InitializeOrleansSilo();
            silo.StartOrleansSilo();

            Console.WriteLine("Silo started.");

            // Then configure and connect a client.
            var clientConfig = ClientConfiguration.LocalhostSilo();
            clientConfig.ResponseTimeout = TimeSpan.FromMinutes(10);
            var client = new ClientBuilder().UseConfiguration(clientConfig).Build();
            client.Connect().Wait();

            Console.WriteLine("Client connected.");

            //
            // This is the place for your test code.
            //


            var problem = ProblemBuilder.Build(File.ReadAllLines(@"..\..\..\Resources\b_should_be_easy.in"));
            var solution = new Solution(problem.NumberOfCars);
            var state = new CityState(problem.Cars.ToImmutableList(), new RidesView3(problem.Rides, problem.Bonus), 0);
            var tree = client.GetGrain<ITreeGrain<MakeRideAction>>(Guid.NewGuid());
            tree.Init(state).Wait();
            tree.Build().Wait();
            var counter = 0;
            INodeView<MakeRideAction> node;
            while ((node = tree.GetTopAction().Result) != null)
            {
                //Trace.WriteLine(node.Action);
                if (!node.Action.Car.Equals(Car.SkipRide))
                {
                    solution.CarActions[node.Action.Car.Id].Add(node.Action);
                }

                if (counter++ % 20 == 0)
                {
                    Trace.WriteLine($"Counter: {counter}");
                    Trace.WriteLine($"Current score: {solution.GetTotalScore(problem.Bonus).ToString()}");
                }

                tree.ContinueFrom(node.Id).Wait();
                tree.Build().Wait();
            }

            Console.WriteLine("Finished");
            Console.WriteLine(solution.GetTotalScore(problem.Bonus).ToString());
            Console.WriteLine(solution.ToString());


            Console.WriteLine("\nPress Enter to terminate...");
            Console.ReadLine();

            // Shut down
            client.Close();
            silo.ShutdownOrleansSilo();
        }
    }
}
