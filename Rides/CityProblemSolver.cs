using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Rides.MCTS;

namespace Rides
{
    public class CityProblemSolver
    {
        public static Solution Solve(Problem problem)
        {
            var solution = new Solution(problem.NumberOfCars);
            var counter = 0;
            IState<MakeRideAction> state = new CityState(problem.Cars.ToImmutableList(), new RidesView3(problem.Rides, problem.Bonus), 0);
            var node = MonteCarloTreeSearch<MakeRideAction>.Create(state);
            while ((node = MonteCarloTreeSearch<MakeRideAction>.GetTopActions(node, 1000, long.MaxValue).FirstOrDefault()) != null)
            {
                node.Parent = null;
                if (!node.Action.Car.Equals(Car.SkipRide))
                {
                    solution.CarActions[node.Action.Car.Id].Add(node.Action);
                }

                //Trace.WriteLine("");
                //Trace.WriteLine($"SELECTED ACTION {node.Action}");
                //Trace.WriteLine("");
                counter++;
                if (counter % 100 == 0)
                {
                    Trace.WriteLine(counter);
                    Console.WriteLine(counter);
                }
            }

            return solution;
        }
    }
}