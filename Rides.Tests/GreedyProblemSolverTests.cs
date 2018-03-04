using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Rides.Tests
{
    public class GreedyProblemSolverTests
    {
        public GreedyProblemSolverTests(ITestOutputHelper outputHelper)
        {
            Trace.Listeners.Add(new TestTraceListener(outputHelper));
        }

        [Fact]
        public void ATest()
        {
            var problem = ProblemBuilder.Build(File.ReadAllLines(@"..\..\..\Resources\a_example.in"));
            var solution = GreedyProblemSolver.Solve(problem);
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
            var solution = GreedyProblemSolver.Solve(problem);
            Assert.NotNull(solution);
            Trace.WriteLine(solution.GetTotalScore(problem.Bonus).ToString());
            Trace.WriteLine($"Missed rides: {string.Join(" ", solution.GetMissedRides())}");
            Trace.WriteLine(solution.ToString());
        }

        [Fact]
        public void CTest()
        {
            var problem = ProblemBuilder.Build(File.ReadAllLines(@"..\..\..\Resources\c_no_hurry.in"));
            var solution = GreedyProblemSolver.Solve(problem);
            Assert.NotNull(solution);
            Trace.WriteLine(solution.GetTotalScore(problem.Bonus).ToString());
            Trace.WriteLine(solution.ToString());
        }

        [Fact]
        public void DTest()
        {
            var problem = ProblemBuilder.Build(File.ReadAllLines(@"..\..\..\Resources\d_metropolis.in"));
            var solution = GreedyProblemSolver.Solve(problem);
            Assert.NotNull(solution);
            Trace.WriteLine(solution.GetTotalScore(problem.Bonus).ToString());
            Trace.WriteLine(solution.ToString());
        }
    }
}