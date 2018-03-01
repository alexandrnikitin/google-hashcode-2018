using System.Collections.Generic;

namespace Pizza
{
    public class Problem
    {
        public int Rows { get; }
        public int Columns { get; }
        public int Fleet { get; }
        public int Rides { get; }
        public int Bonus { get; }
        public int Steps { get; }
        public List<Ride> RidesList { get; }

        public Problem(int rows, int columns, int fleet, int rides, int bonus, int steps, List<Ride> ridesList)
        {
            Rows = rows;
            Columns = columns;
            Fleet = fleet;
            Rides = rides;
            Bonus = bonus;
            Steps = steps;
            RidesList = ridesList;
        }
    }
}