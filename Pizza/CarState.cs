using System;

namespace Pizza
{
    public class CarState
    {
        public int Id { get; set; }
        public Point Location { get; set; }
        public int CurrentTime { get; set; }
    }

    public class Point
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

    public class Ride
    {
        public Point From { get; set; }
        public Point To { get; set; }

        public int Pickup { get; set; }
        public int Finish { get; set; }

        public int WaitTime(CarState carState)
        {
            return Pickup - carState.CurrentTime - carState.Location.Distance(From);
        }
    }
}
