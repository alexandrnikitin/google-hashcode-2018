using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Rides;

namespace RidesApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var problem = ProblemBuilder.Build(File.ReadAllLines(@"..\..\..\Resources\a_example.in"));
            var solution = CityProblemSolver.Solve(problem);
            Console.WriteLine($"a: {solution.GetTotalScore(problem.Bonus).ToString()}");
            File.WriteAllText(@"..\..\..\Resources\a_example.out", solution.ToString());

            problem = ProblemBuilder.Build(File.ReadAllLines(@"..\..\..\Resources\b_should_be_easy.in"));
            solution = CityProblemSolver.Solve(problem);
            Console.WriteLine($"b: {solution.GetTotalScore(problem.Bonus).ToString()}");
            File.WriteAllText(@"..\..\..\Resources\b_should_be_easy.out", solution.ToString());

            problem = ProblemBuilder.Build(File.ReadAllLines(@"..\..\..\Resources\c_no_hurry.in"));
            solution = CityProblemSolver.Solve(problem);
            Console.WriteLine($"c: {solution.GetTotalScore(problem.Bonus).ToString()}");
            File.WriteAllText(@"..\..\..\Resources\c_no_hurry.out", solution.ToString());

            problem = ProblemBuilder.Build(File.ReadAllLines(@"..\..\..\Resources\d_metropolis.in"));
            solution = CityProblemSolver.Solve(problem);
            Console.WriteLine($"d: {solution.GetTotalScore(problem.Bonus).ToString()}");
            File.WriteAllText(@"..\..\..\Resources\d_metropolis.out", solution.ToString());

            problem = ProblemBuilder.Build(File.ReadAllLines(@"..\..\..\Resources\e_high_bonus.in"));
            solution = CityProblemSolver.Solve(problem);
            Console.WriteLine($"e: {solution.GetTotalScore(problem.Bonus).ToString()}");
            File.WriteAllText(@"..\..\..\Resources\e_high_bonus.out", solution.ToString());

        }
    }
}
