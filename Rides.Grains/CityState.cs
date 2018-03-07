﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Orleans.Concurrency;
using Rides.GrainInterfaces;

namespace Rides.Grains
{
    [Immutable]
    [Serializable]
    public class CityState : IState<MakeRideAction>
    {
        public ImmutableList<Car> Cars { get; set; }
        public RidesView3 Rides { get; set; }
        private double _score;

        public CityState(ImmutableList<Car> cars, RidesView3 rides)
        {
            Cars = cars;
            Rides = rides;
        }

        public IEnumerable<MakeRideAction> GetAvailableActions()
        {
            if (Rides.Count == 0) return new List<MakeRideAction>(0);
            var ride = Rides.GetEarliestFinish();
            if (ride.Equals(default)) return new List<MakeRideAction>(0);

            var actions = new List<MakeRideAction>(Cars.Count + 1);

            foreach (var car in Cars)
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
            var newRides = Rides.Remove(action.Ride);
            if (action.Car.Equals(Car.SkipRide))
            {
                return new CityState(Cars, newRides);
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
            var newCars = Cars.Replace(action.Car, newCar);
            return new CityState(newCars, newRides);

        }

        public double GetScore()
        {
            return _score;
        }
    }
}