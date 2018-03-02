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
        public void ATest()
        {
            var problem = ProblemBuilder.Build(File.ReadAllLines(@"..\..\..\Resources\a_example.in"));
            var solution = ProblemSolver.Solve(problem);
            Assert.NotNull(solution);
            Trace.WriteLine(solution.ToString());
            File.WriteAllText(@"..\..\..\Resources\a_example.out", solution.ToString());
        }

        [Fact]
        public void BTest()
        {
            var problem = ProblemBuilder.Build(File.ReadAllLines(@"..\..\..\Resources\b_should_be_easy.in"));
            var solution = ProblemSolver.Solve(problem);
            Assert.NotNull(solution);
            Trace.WriteLine(solution.ToString());
            File.WriteAllText(@"..\..\..\Resources\b_should_be_easy.out", solution.ToString());
        }

        [Fact]
        public void CTest()
        {
            var problem = ProblemBuilder.Build(File.ReadAllLines(@"..\..\..\Resources\c_no_hurry.in"));
            var solution = ProblemSolver.Solve(problem);
            Assert.NotNull(solution);
            Trace.WriteLine(solution.ToString());
            File.WriteAllText(@"..\..\..\Resources\c_no_hurry.out", solution.ToString());
        }

        [Fact]
        public void DTest()
        {
            var problem = ProblemBuilder.Build(File.ReadAllLines(@"..\..\..\Resources\d_metropolis.in"));
            var solution = ProblemSolver.Solve(problem);
            Assert.NotNull(solution);
            Trace.WriteLine(solution.ToString());
            File.WriteAllText(@"..\..\..\Resources\d_metropolis.out", solution.ToString());
        }

        [Fact]
        public void ETest()
        {
            var problem = ProblemBuilder.Build(File.ReadAllLines(@"..\..\..\Resources\e_high_bonus.in"));
            var solution = ProblemSolver.Solve(problem);
            Assert.NotNull(solution);
            Trace.WriteLine(solution.ToString());
            File.WriteAllText(@"..\..\..\Resources\e_high_bonus.out", solution.ToString());
        }
    }
}