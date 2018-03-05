using System;
using System.Collections.Generic;
using System.Linq;
using Rides.MCTS;

namespace Rides
{
    public class CityState : IState<MakeRideAction>
    {
        private readonly Problem _problem;
        private readonly List<Car> _cars;
        private readonly RidesView2 _rides;
        private List<MakeRideAction> _actions;

        public double Score { get; set; }

        public CityState(Problem problem, List<Car> cars, RidesView2 rides, double score)
        {
            _problem = problem;
            _cars = cars;
            _rides = rides;
            Score = score;
            _actions = new List<MakeRideAction>(_cars.Count + 1);
        }

        public object Clone()
        {
            return new CityState(
                _problem, 
                new List<Car>(_cars), 
                (RidesView2) _rides.Clone(), 
                Score);
        }

        public List<MakeRideAction> GetAvailableActions()
        {
            if (_rides.Count == 0) return new List<MakeRideAction>(0);
            var ride = _rides.GetEarliestFinish();
            if (ride.Equals(default(Ride)))
            {
                return new List<MakeRideAction>(0);
            }
            _actions.Clear();
            
            foreach (var car in _cars)
            {
                if (car.CanMakeIt(ride))
                {
                    _actions.Add(new MakeRideAction(ride, car));
                }
            }

            _actions.Add(new MakeRideAction(ride, Car.SkipRide));
            return _actions.OrderByDescending(x => x.GetScore(_problem.Bonus)).Take(10).ToList();
            return _actions;
        }


        public void ApplyAction(MakeRideAction action)
        {
            if (action.Car.Equals(Car.SkipRide))
            {
                _rides.Remove(action.Ride);
                return;
            }

            Score += action.GetFactScore(_problem.Bonus);
            _rides.Remove(action.Ride);

            var rideDistance = action.Ride.From.Distance(action.Ride.To);
            var pickDistance = action.Car.Location.Distance(action.Ride.From);
            var waitTime = Math.Max(0, action.Ride.Start - (action.Car.Time + pickDistance)); // todo check same time
            var totalRideTime = pickDistance + rideDistance + waitTime;
            _cars[action.Car.Id] =
                new Car(
                    action.Car.Id,
                    new Point(action.Ride.To.X, action.Ride.To.Y),
                    action.Car.Time + totalRideTime); ;
        }

        public double GetScore()
        {
            return Score;
        }
    }
}