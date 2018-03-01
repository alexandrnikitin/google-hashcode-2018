using System.Collections.Generic;

namespace Pizza
{
    public class ProblemSolver
    {
        public static Solution Solve(Problem problem)
        {
            var solution = new Solution(problem.Fleet);

            return solution;
        }
    }
}