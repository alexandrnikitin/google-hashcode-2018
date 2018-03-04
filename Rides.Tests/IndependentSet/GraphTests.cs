using System.Diagnostics;
using System.IO;
using Rides.IndependentSet;
using Xunit;
using Xunit.Abstractions;

namespace Rides.Tests.IndependentSet
{
    public class GraphTests
    {
        public GraphTests(ITestOutputHelper outputHelper)
        {
            Trace.Listeners.Add(new TestTraceListener(outputHelper));
        }

        [Fact]
        public void ABuildTest()
        {
            var problem = ProblemBuilder.Build(File.ReadAllLines(@"..\..\..\Resources\a_example.in"));
            var graph = Graph.Build(problem.Rides);
            Assert.NotNull(graph);
            Assert.Equal(3, graph.Vertices.Count);

            foreach (var vertex in graph.Vertices)
            {
                Trace.WriteLine(vertex);
            }
            foreach (var edge in graph.Edges)
            {
                Trace.WriteLine(edge);
            }
        }

        [Fact]
        public void CBuildTest()
        {
            var problem = ProblemBuilder.Build(File.ReadAllLines(@"..\..\..\Resources\c_no_hurry.in"));
            var graph = Graph.Build(problem.Rides);
            Assert.NotNull(graph);
            Assert.Equal(10000, graph.Vertices.Count);
            Trace.WriteLine(graph.Edges.Count);
        }
    }
}