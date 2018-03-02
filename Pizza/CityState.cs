using System;
using System.Collections.Generic;
using Pizza.MonteCarloTreeSearch;

namespace Pizza
{
    public class CityState : IState<Ride>
    {
        private readonly HashSet<Ride> _ridesAvailable;
        private readonly CarState _carState;
        private readonly int _bonus;
        private int _score;

        public CityState(HashSet<Ride> ridesAvailable, CarState carState, int bonus)
        {
            _ridesAvailable = ridesAvailable;
            _carState = carState;
            _bonus = bonus;
        }

        public IState<Ride> Clone()
        {
            var ra = new HashSet<Ride>(_ridesAvailable);
            var cs = new CarState {Id = _carState.Id, Location = _carState.Location, Time = _carState.Time};
            return new CityState(ra, cs, _bonus);
        }

        public HashSet<Ride> GetAvailableActions()
        {
            return _ridesAvailable;
        }

        public void ApplyAction(Ride ride)
        {
            var startTime = Math.Max(ride.Start, _carState.Time + _carState.Location.Distance(ride.From));
            _carState.Location = ride.To;
            var distance = ride.From.Distance(ride.To);
            _carState.Time = startTime + distance;

            _score += distance;
            if (startTime == ride.Start)
                _score += _bonus;

            _ridesAvailable.Remove(ride);
            _ridesAvailable.RemoveWhere(x => !_carState.CanMakeIt(x));
        }

        public double GetResult()
        {
            return _score;
        }
    }

    public class RideAction : IAction
    {
        public RideAction(Ride ride, CarState car)
        {
            Ride = ride;
            Car = car;
        }

        public Ride Ride { get; }
        public CarState Car { get; }
    }
}