using System;
using System.Collections.Generic;

namespace Rides
{
    public class RidesView
    {
        private readonly SortedSet<Ride> _earliestFinish;
        //private readonly SortedSet<Ride> _earliestStart;

        public RidesView(IEnumerable<Ride> rides)
        {
            //var rides0 = rides.ToList();
            _earliestFinish = new SortedSet<Ride>(rides, EarliestComparer);
            //_earliestStart = new SortedSet<Ride>(rides0, EarliestStartComparer);
        }

        public IEnumerable<Ride> GetRides()
        {
            return _earliestFinish;
        }

        public Ride GetEarliestFinish()
        {
            return _earliestFinish.Min;
        }

        public Ride GetEarliestStart()
        {
            //return _earliestStart.Min;
            throw new NotImplementedException();
        }

        public void Remove(Ride ride)
        {
            //_earliestStart.Remove(ride);
            _earliestFinish.Remove(ride);
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
        private sealed class EarliestStartRelationalComparer : IComparer<Ride>
        {
            public int Compare(Ride x, Ride y)
            {
                return x.Start == y.Start ? x.Id.CompareTo(y.Id) : x.Start.CompareTo(y.Start);
            }
        }

        public static IComparer<Ride> EarliestComparer { get; } = new EarliestRelationalComparer();
        public static IComparer<Ride> EarliestStartComparer { get; } = new EarliestStartRelationalComparer();
    }
}