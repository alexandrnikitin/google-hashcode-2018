using System;
using System.Collections.Generic;
using Rides.MCTS;

namespace Rides
{
    public class CityState : IState<MakeRideAction>
    {
        private readonly Problem _problem;
        private readonly HashSet<Car> _availableCars;
        private readonly HashSet<Car> _busyCars;
        private readonly HashSet<Ride> _availableRides;
        private int _score;

        public CityState(Problem problem, HashSet<Car> availableCars, HashSet<Car> busyCars, HashSet<Ride> availableRides)
        {
            _problem = problem;
            _availableCars = availableCars;
            _busyCars = busyCars;
            _availableRides = availableRides;
        }

        public object Clone()
        {
            return new CityState(_problem, 
                new HashSet<Car>(_availableCars), 
                new HashSet<Car>(_busyCars),
                new HashSet<Ride>(_availableRides));
        }

        public IEnumerable<MakeRideAction> GetAvailableActions()
        {
            foreach (var busyCar in _busyCars)
            {
                
            }

            var set = new SortedSet<MakeRideAction>(new MakeRideActionComparer(_problem));
            foreach (var car in _availableCars)
            {
                foreach (var ride in _availableRides)
                {
                    set.Add(new MakeRideAction(ride, car));
                }
            }

            return set.Reverse();
        }

        public void ApplyAction(MakeRideAction action)
        {
            _score += action.GetScore(_problem.Bonus);
            _availableRides.Remove(action.Ride);
            _availableCars.Remove(action.Car);




            var rideDistance = action.Ride.From.Distance(action.Ride.To);
            var pickDistance = action.Car.Location.Distance(action.Ride.From);
            var waitTime = Math.Max(0, action.Ride.Start - (action.Car.Time + pickDistance));
            var totalRideTime = pickDistance + rideDistance + waitTime;
            _busyCars.Add(
                new Car(
                    action.Car.Id,
                    new Point(action.Ride.To.X, action.Ride.To.Y),
                    action.Car.Time + totalRideTime));
        }

        public double GetScore()
        {
            return _score;
        }
    }
}