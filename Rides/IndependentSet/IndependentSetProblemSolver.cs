namespace Rides.IndependentSet
{
    public class IndependentSetProblemSolver
    {
        public static Solution Solve(Problem problem)
        {
            var solution = new Solution(problem.NumberOfCars);
            var graph = Graph.Build(problem.Rides);

            foreach (var car in problem.Cars)
            {
                var earliest = graph.GetEarliestFinishRide();
                while (earliest != null)
                {
                    
                }
            }

            


            return solution;
        }
    }
}