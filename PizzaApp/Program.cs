using System;
using System.IO;
using Pizza;

namespace PizzaApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var problem = ProblemBuilder.Build(File.ReadAllLines(@"..\..\..\Resources\big.in"));
            var solution = ProblemSolver.Solve(problem);
            File.WriteAllText(@"..\..\..\Resources\big.out", solution.ToString());
        }
    }
}