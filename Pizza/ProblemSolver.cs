using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Pizza.MonteCarloTreeSearch;

namespace Pizza
{
    public class ProblemSolver
    {
        public static Solution Solve(Problem problem)
        {
            var solution = new Solution(problem.Fleet);
            var rides = new List<Ride>(problem.RidesList);
            var counter = 0;

            foreach (var car in problem.Cars)
            {
                var game = new CityState(new HashSet<Ride>(rides), car, problem.Bonus);
                IMctsNode<Ride> action;
                while ((action = MonteCarloTreeSearch.MonteCarloTreeSearch.GetTopActions(game, 10000, 100).FirstOrDefault()) != null)
                {
                    game.ApplyAction(action.Action);
                    solution.Vehicles[car.Id].Add(action.Action.Id);
                    counter++;
                    if (counter % 100 == 0)
                    {
                        Trace.WriteLine("+");
                        Console.WriteLine("+");
                    }
                }

                rides.RemoveAll(x => solution.Vehicles[car.Id].Contains(x.Id));
            }

            return solution;
        }
    }
}