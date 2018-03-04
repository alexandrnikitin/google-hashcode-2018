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
            var counter = 0;
            var rides = new HashSet<Ride>(problem.Rides);

            foreach (var car in problem.Cars)
            {
                INode<MakeRideAction> node;
                var state = new CityCarState(problem, car, new HashSet<Ride>(rides), 0);
                while ((node = MonteCarloTreeSearch.GetTopActions(state, 100, 100).FirstOrDefault()) != null)
                {
                    state.ApplyAction(node.Action);
                    solution.CarActions[node.Action.Car.Id].Add(node.Action);
                    rides.Remove(node.Action.Ride);
                    //Trace.WriteLine($"{node.Action}");
                    counter++;
                    if (counter % 100 == 0)
                    {
                        Trace.WriteLine("+");
                        Console.WriteLine("+");
                    }
                }
            }

            return solution;
        }
    }
}