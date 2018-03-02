using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rides
{
    public class Solution
    {
        public List<List<MakeRideAction>> CarActions { get; set; }

        public Solution(int numberOfCars)
        {
            CarActions = new List<List<MakeRideAction>>(numberOfCars);
            for (var i = 0; i < numberOfCars; i++)
            {
                CarActions.Add(new List<MakeRideAction>());
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var v in CarActions)
            {
                sb.AppendLine($"{v.Count} {string.Join(" ", v.Select(x => x.Ride.Id))}");
            }

            return sb.ToString();
        }

        public int GetTotalScore(int bonus)
        {
            var score = 0;
            foreach (var actions in CarActions.Where(x => x.Any()))
            {
                foreach (var action in actions)
                {
                    var rideDistance = action.Ride.From.Distance(action.Ride.To);
                    var pickDistance = action.Car.Location.Distance(action.Ride.From);
                    score += rideDistance;
                    var timeToPick = action.Car.Time + pickDistance;
                    var onTime = timeToPick <= action.Ride.Start;

                    if (onTime) score += bonus;
                }
            }

            return score;
        }
    }
}