using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Rides.MCTS;

namespace Rides
{
    public class ProblemSolver
    {
        public static Solution Solve(Problem problem)
        {
            var solution = new Solution(problem.NumberOfCars);
            INode<MakeRideAction> action;
            var state = new CityState(problem, new HashSet<Car>(problem.Cars), new HashSet<Ride>(problem.Rides));
            var counter = 0;

            while ((action = MonteCarloTreeSearch.GetTopActions(state, 10000, 100).FirstOrDefault()) != null)
            {
                state.ApplyAction(action.Action);
                solution.CarActions[action.Action.Car.Id].Add(action.Action);
                counter++;
                if (counter % 100 == 0)
                {
                    Trace.WriteLine("+");
                    Console.WriteLine("+");
                }
            }

            return solution;
        }
    }
}