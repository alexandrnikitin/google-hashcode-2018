using System;
using System.Collections.Generic;
using System.Linq;
using Rides.MCTS;

namespace Rides
{
    public class CityCarState : IState<MakeRideAction>
    {
        private readonly Problem _problem;
        private Car _car;
        private readonly HashSet<Ride> _availableRides;
        private double _score;

        public CityCarState(Problem problem, Car car, HashSet<Ride> availableRides, double score)
        {
            _problem = problem;
            _car = car;
            _availableRides = availableRides;
            _score = score;
        }

        public object Clone()
        {
            return new CityCarState(_problem, 
                _car, 
                new HashSet<Ride>(_availableRides),
                _score);
        }

        public List<MakeRideAction> GetAvailableActions()
        {
            var set = new HashSet<MakeRideAction>();
            foreach (var ride in GetMinFinish(10))
            {
                set.Add(new MakeRideAction(ride, _car));
            }
            foreach (var ride in GetMinPickup(10))
            {
                set.Add(new MakeRideAction(ride, _car));
            }
            foreach (var ride in GetBestScore(10))
            {
                set.Add(new MakeRideAction(ride, _car));
            }

            return set.OrderByDescending(x => x.GetScore(_problem.Bonus)).ToList();
        }

        private IEnumerable<Ride> GetMinFinish(int n)
        {
            var comparer = Ride.FinishComparer;
            var heap = new MaxHeap<Ride>(comparer);
            foreach (var ride in _availableRides)
            {
                if (_car.CanMakeIt(ride) && (heap.Count < n || comparer.Compare(heap.Max, ride) > 0))
                {
                    heap.Add(ride);
                    if (heap.Count > n) heap.ExtractMax();
                }
            }

            return heap;
        }

        private IEnumerable<Ride> GetMinPickup(int n)
        {
            var comparer = Ride.CreatePickUpComparer(_car);
            var heap = new MaxHeap<Ride>(comparer);
            foreach (var ride in _availableRides)
            {
                if (_car.CanMakeIt(ride) && (heap.Count < n || comparer.Compare(heap.Max, ride) > 0))
                {
                    heap.Add(ride);
                    if (heap.Count > n) heap.ExtractMax();
                }
            }

            return heap;
        }

        private IEnumerable<Ride> GetBestScore(int n)
        {
            var comparer = Ride.CreateScoreComparer(_car, _problem);
            var heap = new MinHeap<Ride>(comparer);
            foreach (var ride in _availableRides)
            {
                if (_car.CanMakeIt(ride) && (heap.Count < n || comparer.Compare(heap.Min, ride) < 0))
                {
                    heap.Add(ride);
                    if (heap.Count > n) heap.ExtractMin();
                }
            }

            return heap;
        }


        public void ApplyAction(MakeRideAction action)
        {
            _score += action.GetFactScore(_problem.Bonus);
            _availableRides.Remove(action.Ride);

            var rideDistance = action.Ride.From.Distance(action.Ride.To);
            var pickDistance = action.Car.Location.Distance(action.Ride.From);
            var waitTime = Math.Max(0, action.Ride.Start - (action.Car.Time + pickDistance));
            var totalRideTime = pickDistance + rideDistance + waitTime;
            _car = 
                new Car(
                    action.Car.Id,
                    new Point(action.Ride.To.X, action.Ride.To.Y),
                    action.Car.Time + totalRideTime);

            _availableRides.RemoveWhere(x => !_car.CanMakeIt(x));
        }

        public double GetScore()
        {
            return _score;
        }
    }
}