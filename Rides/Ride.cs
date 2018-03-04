using System;
using System.Collections.Generic;

namespace Rides
{
    public struct Ride : IEquatable<Ride>
    {
        public int Id { get; }
        public Point From { get; }
        public Point To { get; }
        public int Start { get; }
        public int Finish { get; }

        public Ride(int id, Point @from, Point to, int start, int finish)
        {
            Id = id;
            From = @from;
            To = to;
            Start = start;
            Finish = finish;
        }

        public bool Equals(Ride other)
        {
            return Id == other.Id && From.Equals(other.From) && To.Equals(other.To) && Start == other.Start && Finish == other.Finish;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Ride && Equals((Ride) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode * 397) ^ From.GetHashCode();
                hashCode = (hashCode * 397) ^ To.GetHashCode();
                hashCode = (hashCode * 397) ^ Start;
                hashCode = (hashCode * 397) ^ Finish;
                return hashCode;
            }
        }

        private sealed class FinishRelationalComparer : IComparer<Ride>
        {
            public int Compare(Ride x, Ride y)
            {
                return x.Finish.CompareTo(y.Finish);
            }
        }

        private sealed class PickupRelationalComparer : IComparer<Ride>
        {
            private readonly Car _car;

            public PickupRelationalComparer(Car car)
            {
                _car = car;
            }

            public int Compare(Ride x, Ride y)
            {
                var pickDistance = _car.Location.Distance(x.From);
                var waitTime = Math.Max(0, x.Start - (_car.Time + pickDistance));
                var pickupX = pickDistance + waitTime;

                pickDistance = _car.Location.Distance(y.From);
                waitTime = Math.Max(0, y.Start - (_car.Time + pickDistance));
                var pickupY = pickDistance + waitTime;

                return pickupX.CompareTo(pickupY);
            }
        }

        private sealed class ScoreRelationalComparer : IComparer<Ride>
        {
            private readonly Car _car;
            private readonly Problem _problem;

            public ScoreRelationalComparer(Car car, Problem problem)
            {
                _car = car;
                _problem = problem;
            }

            public int Compare(Ride x, Ride y)
            {
                return MakeRideAction.GetScore(x, _car, _problem.Bonus)
                    .CompareTo(MakeRideAction.GetScore(y, _car, _problem.Bonus));
            }
        }

        public static IComparer<Ride> FinishComparer { get; } = new FinishRelationalComparer();
        public static IComparer<Ride> CreatePickUpComparer(Car car) => new PickupRelationalComparer(car);
        public static IComparer<Ride> CreateScoreComparer(Car car, Problem problem) => new ScoreRelationalComparer(car, problem);

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(From)}: {From}, {nameof(To)}: {To}, {nameof(Start)}: {Start}, {nameof(Finish)}: {Finish}, Distance: {From.Distance(To)}";
        }

    }
}