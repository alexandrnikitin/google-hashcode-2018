using System.Collections.Generic;

namespace Rides.IndependentSet
{
    public class Vertex
    {
        public Ride Ride { get; }

        public Vertex(Ride ride)
        {
            Ride = ride;
        }

        private sealed class FinishRelationalComparer : IComparer<Vertex>
        {
            public int Compare(Vertex x, Vertex y)
            {
                var finishX = x.Ride.Start + x.Ride.From.Distance(x.Ride.To);
                var finishY = y.Ride.Start + y.Ride.From.Distance(y.Ride.To);
                return finishX == finishY ? x.Ride.Id.CompareTo(y.Ride.Id) : finishX.CompareTo(finishY);
            }
        }

        public static IComparer<Vertex> FinishComparer { get; } = new FinishRelationalComparer();

        public override string ToString()
        {
            return $"Vertex: {nameof(Ride)}: {Ride}";
        }
    }
}