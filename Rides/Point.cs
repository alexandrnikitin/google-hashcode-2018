using System;

namespace Rides
{
    public struct Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int Distance(Point other) => Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
    }
}