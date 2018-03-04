using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Rides
{
    public class GreedyProblemSolver
    {
        public static Solution Solve(Problem problem)
        {
            var solution = new Solution(problem.NumberOfCars);
            var rides = new HashSet<Ride>(problem.Rides);

            foreach (var car in problem.Cars)
            {
                var car0 = car;
                var rides0 = new HashSet<Ride>(rides);
                Trace.WriteLine(car0);
                while (rides0.Count > 0)
                {
                    Trace.WriteLine(rides0.Count);
                    Ride? min = null;
                    foreach (var ride in rides)
                    {
                        if (car0.CanMakeIt(ride) && (!min.HasValue || ride.Finish < min.Value.Finish))
                        {
                            min = ride;
                        }
                    }

                    if (min.HasValue)
                    {
                        var action = new MakeRideAction(min.Value, car0);
                        solution.CarActions[action.Car.Id].Add(action);
                        rides.Remove(action.Ride);
                        var rideDistance = action.Ride.From.Distance(action.Ride.To);
                        var pickDistance = action.Car.Location.Distance(action.Ride.From);
                        var waitTime = Math.Max(0, action.Ride.Start - (action.Car.Time + pickDistance));
                        var totalRideTime = pickDistance + rideDistance + waitTime;

                        car0 =
                            new Car(
                                action.Car.Id,
                                new Point(action.Ride.To.X, action.Ride.To.Y),
                                action.Car.Time + totalRideTime);

                        rides0.RemoveWhere(x => !car0.CanMakeIt(x));
                    }
                }
            }

            return solution;
        }
    }
}