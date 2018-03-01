using System.Diagnostics;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Pizza.Tests
{
    public class ProblemBuilderTests
    {
        public ProblemBuilderTests(ITestOutputHelper outputHelper)
        {
            Trace.Listeners.Add(new TestTraceListener(outputHelper));
        }

        [Fact]
        public void Test()
        {
            var lines = File.ReadAllLines(@"..\..\..\Resources\example.in");
            var problem = ProblemBuilder.Build(lines);
            Assert.NotNull(problem);
            Trace.WriteLine(problem.Pizza.ToString());
        }
    }
}