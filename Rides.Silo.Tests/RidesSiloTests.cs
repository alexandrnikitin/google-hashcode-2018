using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;
using Orleans.Runtime.Configuration;
using Orleans.TestingHost;
using Rides.GrainInterfaces;
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
            var state = new CityState(problem, problem.Cars, new RidesView3(problem.Rides), 0);
            var grain = GrainFactory.GetGrain<IMonteCarloTreeSearchGrain<MakeRideAction>>(Guid.NewGuid());
            await grain.Init(state);
            var reply = await grain.GetTopAction(10, 10);
            Assert.NotNull(reply);
            Trace.WriteLine(reply.Action);
        }


        public class OrleansSiloFixture : IDisposable
        {
            public TestCluster Cluster { get; }

            public OrleansSiloFixture()
            {
                GrainClient.Uninitialize();

                var options = new TestClusterOptions(initialSilosCount: 2);
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
