using System;
using Pizza.MonteCarloTreeSearch;

namespace Pizza
{
    public class CarState
    {
        public int Id { get; set; }
        public Point Location { get; set; }
        public int Time { get; set; }

        public bool CanMakeIt(Ride ride)
        {
            return ride.Finish >= Time + Location.Distance(ride.From) + ride.From.Distance(ride.To);
        }
    }

    public struct Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int Distance(Point other)
        {
            return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
        }
    }

    public struct Ride : IAction
    {
        public int Id { get; set; }
        public Point From { get; set; }
        public Point To { get; set; }

        public int Start { get; set; }
        public int Finish { get; set; }

        public int WaitTime(CarState carState)
        {
            return Start - carState.Time - carState.Location.Distance(From);
        }
    }
}