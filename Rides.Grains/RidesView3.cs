using System;
using System.Collections.Generic;
using System.Linq;

namespace Rides.Grains
{
    [Serializable]
    public class RidesView3
    {
        private int? _lastRemoved;

        private readonly List<Ride> _earliestFinish;

        public RidesView3(IEnumerable<Ride> rides)
        {
            _earliestFinish = rides.OrderBy(x => x, EarliestComparer).ToList();
        }

        private RidesView3(List<Ride> earliestFinish, int? lastRemoved)
        {
            _earliestFinish = earliestFinish;
            _lastRemoved = lastRemoved;
        }

        public Ride GetEarliestFinish()
        {
            if (_lastRemoved.HasValue)
            {
                if (_lastRemoved.Value + 1 < _earliestFinish.Count)
                {
                    return _earliestFinish[_lastRemoved.Value + 1];
                }
                else
                {
                    return default;
                }
            }
            else
                return _earliestFinish[0];
        }

        public RidesView3 Remove(Ride ride)
        {
            return _lastRemoved.HasValue
                ? new RidesView3(_earliestFinish, _lastRemoved.Value + 1)
                : new RidesView3(_earliestFinish, 0);
        }

        public int Count => _lastRemoved.HasValue ? _earliestFinish.Count - _lastRemoved.Value : _earliestFinish.Count - 1;

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

    }
}