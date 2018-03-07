using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Orleans;
using Orleans.Runtime.Configuration;
using Orleans.TestingHost;
using Rides.GrainInterfaces;
using Rides.Grains;
using Xunit;
using Xunit.Abstractions;

namespace Rides.Silo.Tests
{
    public class RidesSiloTests : IClassFixture<RidesSiloTests.OrleansSiloFixture>
    {
        private readonly OrleansSiloFixture _fixture;

        public RidesSiloTests(OrleansSiloFixture fixture, ITestOutputHelper outputHelper)
        {
            _fixture = fixture;
            Trace.Listeners.Add(new TestTraceListener(outputHelper));
        }

        private IGrainFactory GrainFactory => _fixture.Cluster.GrainFactory;

        [Fact]
        public async Task SiloSayHelloTest()
        {
            var problem = ProblemBuilder.Build(File.ReadAllLines(@"..\..\..\Resources\a_example.in"));
            var state = new CityState(problem.Cars.ToImmutableList(), new RidesView3(problem.Rides));
            var grain = GrainFactory.GetGrain<ITreeGrain<MakeRideAction>>(Guid.NewGuid());
            await grain.Init(state);
            await grain.Build();
            var bestNode = await grain.GetTopAction();
            Assert.NotNull(bestNode);
            Trace.WriteLine(bestNode.Action);
        }

        [Fact]
        public async Task ATest()
        {
            var problem = ProblemBuilder.Build(File.ReadAllLines(@"..\..\..\Resources\a_example.in"));
            var solution = new Solution(problem.NumberOfCars);
            var state = new CityState(problem.Cars.ToImmutableList(), new RidesView3(problem.Rides));
            var tree = GrainFactory.GetGrain<ITreeGrain<MakeRideAction>>(Guid.NewGuid());
            await tree.Init(state);
            await tree.Build();
            INodeView<MakeRideAction> node;
            while ((node = await tree.GetTopAction()) != null)
            {
                Trace.WriteLine(node.Action);
                if (!node.Action.Car.Equals(Car.SkipRide))
                {
                    solution.CarActions[node.Action.Car.Id].Add(node.Action);
                }
                await tree.ContinueFrom(node.Id);
                await tree.Build();
            }

            Assert.NotNull(solution);
            Trace.WriteLine("Finished");
            Trace.WriteLine(solution.GetTotalScore(problem.Bonus).ToString());
            Trace.WriteLine(solution.ToString());
            Assert.Equal(10, solution.GetTotalScore(problem.Bonus));

        }


        public class OrleansSiloFixture : IDisposable
        {
            public TestCluster Cluster { get; }

            public OrleansSiloFixture()
            {
                GrainClient.Uninitialize();

                var options = new TestClusterOptions(initialSilosCount: 2);
                options.ClusterConfiguration.Globals.ResponseTimeout = TimeSpan.FromMinutes(10);
                options.ClusterConfiguration.AddMemoryStorageProvider("Default");
                options.ClusterConfiguration.AddMemoryStorageProvider("MemoryStore");
                Cluster = new TestCluster(options);

                if (Cluster.Primary == null)
                {
                    Cluster.Deploy();
                }
            }

            public void Dispose()
            {
                Cluster.StopAllSilos();
            }
        }

    }
}
