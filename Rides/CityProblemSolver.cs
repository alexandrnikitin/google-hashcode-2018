using System;
using System.Collections.Generic;
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
            var state = new CityState(problem, problem.Cars, new RidesView(problem.Rides), 0);

            INode<MakeRideAction> node;
            while ((node = MonteCarloTreeSearch.GetTopActions(state, 1000).FirstOrDefault()) != null)
            {
                state.ApplyAction(node.Action);
                state.Score = 0;
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