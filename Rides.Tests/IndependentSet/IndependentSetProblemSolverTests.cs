using System.Diagnostics;
using System.IO;
using Rides.IndependentSet;
using Xunit;
using Xunit.Abstractions;

namespace Rides.Tests.IndependentSet
{
    public class IndependentSetProblemSolverTests
    {
        public IndependentSetProblemSolverTests(ITestOutputHelper outputHelper)
        {
            Trace.Listeners.Add(new TestTraceListener(outputHelper));
        }

        [Fact]
        public void ATest()
        {
            var problem = ProblemBuilder.Build(File.ReadAllLines(@"..\..\..\Resources\a_example.in"));
            var solution = IndependentSetProblemSolver.Solve(problem);
            Assert.NotNull(solution);
            Trace.WriteLine("Finished");
            Trace.WriteLine(solution.GetTotalScore(problem.Bonus).ToString());
            Trace.WriteLine(solution.ToString());
            Assert.Equal(10, solution.GetTotalScore(problem.Bonus));
        }

        [Fact]
        public void BTest()
        {
            var problem = ProblemBuilder.Build(File.ReadAllLines(@"..\..\..\Resources\b_should_be_easy.in"));
            var solution = IndependentSetProblemSolver.Solve(problem);
            Assert.NotNull(solution);
            Trace.WriteLine(solution.GetTotalScore(problem.Bonus).ToString());
            Trace.WriteLine($"Missed rides: {string.Join(" ", solution.GetMissedRides())}");
            Trace.WriteLine(solution.ToString());
        }
    }
}