using System;
using System.Collections.Generic;
using Rides.MCTS;

namespace Rides
{
    public class MakeRideAction : IAction
    {
        public Ride Ride { get; }
        public Car Car { get; }

        public MakeRideAction(Ride ride, Car car)
        {
            Ride = ride;
            Car = car;
        }

        public int GetScore(int bonus)
        {
            var score = 0;
            var action = this;
            var rideDistance = action.Ride.From.Distance(action.Ride.To);
            var pickDistance = action.Car.Location.Distance(action.Ride.From);
            var waitTime = Math.Max(0, action.Ride.Start - (action.Car.Time + pickDistance));
            var timeToPick = action.Car.Time + pickDistance;
            var onTime = timeToPick <= action.Ride.Start;

            score += rideDistance - pickDistance - waitTime;
            if (onTime) score += bonus;

            return score;
        }

    }

    public class MakeRideActionComparer : IComparer<MakeRideAction>
    {
        private readonly Problem _problem;

        public MakeRideActionComparer(Problem problem)
        {
            _problem = problem;
        }

        public int Compare(MakeRideAction x, MakeRideAction y)
        {
            return x.GetScore(_problem.Bonus) >= y.GetScore(_problem.Bonus) ? 1 : -1;
        }
    }

}