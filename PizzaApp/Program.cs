using System;
using System.IO;
using Pizza;

namespace PizzaApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var problem = ProblemBuilder.Build(File.ReadAllLines(@"..\..\..\Resources\a_example.in"));
            var solution = ProblemSolver.Solve(problem);
            File.WriteAllText(@"..\..\..\Resources\a_example.out", solution.ToString());

            problem = ProblemBuilder.Build(File.ReadAllLines(@"..\..\..\Resources\b_should_be_easy.in"));
            solution = ProblemSolver.Solve(problem);
            File.WriteAllText(@"..\..\..\Resources\b_should_be_easy.out", solution.ToString());

            problem = ProblemBuilder.Build(File.ReadAllLines(@"..\..\..\Resources\c_no_hurry.in"));
            solution = ProblemSolver.Solve(problem);
            File.WriteAllText(@"..\..\..\Resources\c_no_hurry.out", solution.ToString());

            problem = ProblemBuilder.Build(File.ReadAllLines(@"..\..\..\Resources\d_metropolis.in"));
            solution = ProblemSolver.Solve(problem);
            File.WriteAllText(@"..\..\..\Resources\d_metropolis.out", solution.ToString());

            problem = ProblemBuilder.Build(File.ReadAllLines(@"..\..\..\Resources\e_high_bonus.in"));
            solution = ProblemSolver.Solve(problem);
            File.WriteAllText(@"..\..\..\Resources\e_high_bonus.out", solution.ToString());
        }
    }
}