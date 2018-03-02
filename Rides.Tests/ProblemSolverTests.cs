using System.Diagnostics;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Rides.Tests
{
    public class ProblemSolverTests
    {
        public ProblemSolverTests(ITestOutputHelper outputHelper)
        {
            Trace.Listeners.Add(new TestTraceListener(outputHelper));
        }

        [Fact]
        public void ATest()
        {
            var problem = ProblemBuilder.Build(File.ReadAllLines(@"..\..\..\Resources\a_example.in"));
            var solution = ProblemSolver.Solve(problem);
            Assert.NotNull(solution);
            Trace.WriteLine(solution.GetTotalScore(problem.Bonus).ToString());
            Trace.WriteLine(solution.ToString());
        }

        [Fact]
        public void BTest()
        {
            var problem = ProblemBuilder.Build(File.ReadAllLines(@"..\..\..\Resources\b_should_be_easy.in"));
            var solution = ProblemSolver.Solve(problem);
            Assert.NotNull(solution);
            Trace.WriteLine(solution.GetTotalScore(problem.Bonus).ToString());
            Trace.WriteLine(solution.ToString());
        }
    }
}