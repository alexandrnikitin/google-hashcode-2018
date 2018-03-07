using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Orleans.Concurrency;
using Rides.GrainInterfaces;

namespace Rides.Grains
{
    //[Immutable]
    [Serializable]
    public class CityState : IState<MakeRideAction>
    {
        private readonly ImmutableList<Car> _cars;
        private readonly RidesView3 _rides;
        private double _score;

        public CityState(ImmutableList<Car> cars, RidesView3 rides)
        {
            _cars = cars;
            _rides = rides;
        }

        public IEnumerable<MakeRideAction> GetAvailableActions()
        {
            if (_rides.Count == 0) return new List<MakeRideAction>(0);
            var ride = _rides.GetEarliestFinish();
            if (ride.Equals(default)) return new List<MakeRideAction>(0);

            var actions = new List<MakeRideAction>(_cars.Count + 1);

            foreach (var car in _cars)
            {
                if (car.CanMakeIt(ride))
                {
                    actions.Add(new MakeRideAction(ride, car));
                }
            }

            actions.Add(new MakeRideAction(ride, Car.SkipRide));
            return actions;

        }

        public IState<MakeRideAction> Apply(MakeRideAction action)
        {
            var newRides = _rides.Remove(action.Ride);
            if (action.Car.Equals(Car.SkipRide))
            {
                return new CityState(_cars, newRides);
            }

            _score += action.GetFactScore(0); // TODO bonus

            var rideDistance = action.Ride.From.Distance(action.Ride.To);
            var pickDistance = action.Car.Location.Distance(action.Ride.From);
            var waitTime = Math.Max(0, action.Ride.Start - (action.Car.Time + pickDistance)); // todo check same time
            var totalRideTime = pickDistance + rideDistance + waitTime;
            
            var newCar =
                new Car(
                    action.Car.Id,
                    new Point(action.Ride.To.X, action.Ride.To.Y),
                    action.Car.Time + totalRideTime);
            var newCars = _cars.Replace(action.Car, newCar);
            return new CityState(newCars, newRides);

        }

        public double GetScore()
        {
            return _score;
        }
    }
}