using System.Diagnostics;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Pizza.Tests
{
    public class ProblemSolverTests
    {
        public ProblemSolverTests(ITestOutputHelper outputHelper)
        {
            Trace.Listeners.Add(new TestTraceListener(outputHelper));
        }

        [Fact]
        public void ExampleTest()
        {
            var problem = ProblemBuilder.Build(File.ReadAllLines(@"..\..\..\Resources\a_example.in"));
            var solution = ProblemSolver.Solve(problem);
            Assert.NotNull(solution);
            Trace.WriteLine(solution.ToString());
            File.WriteAllText(@"..\..\..\Resources\example.out", solution.ToString());
        }
    }
}