using System;
using System.Collections.Generic;
using System.Linq;

namespace Rides
{
    public class RidesView2 : ICloneable
    {
        private Ride? _lastRemoved;

        private readonly SortedSet<Ride> _earliestFinish;

        public RidesView2(IEnumerable<Ride> rides)
        {
            _earliestFinish = new SortedSet<Ride>(rides, EarliestComparer);
        }

        public RidesView2(SortedSet<Ride> earliestFinish, Ride? lastRemoved)
        {
            _earliestFinish = earliestFinish;
            _lastRemoved = lastRemoved;
        }

        public Ride GetEarliestFinish()
        {
            if (_lastRemoved.HasValue)
            {
                return _earliestFinish.GetViewBetween(_lastRemoved.Value, _earliestFinish.Max).Skip(1).FirstOrDefault();
            }
            else
                return _earliestFinish.Min;
        }

        public void Remove(Ride ride)
        {
            _lastRemoved = ride;
        }

        public int Count => _earliestFinish.Count;

        private sealed class EarliestRelationalComparer : IComparer<Ride>
        {
            public int Compare(Ride x, Ride y)
            {
                var finishX = x.Start + x.From.Distance(x.To);
                var finishY = y.Start + y.From.Distance(y.To);
                return finishX == finishY ? x.Id.CompareTo(y.Id) : finishX.CompareTo(finishY);
            }
        }

        public static IComparer<Ride> EarliestComparer { get; } = new EarliestRelationalComparer();

        public object Clone()
        {
            return new RidesView2(_earliestFinish, _lastRemoved);
        }
    }
}