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

        public Problem(int rows, int columns, int fleet, int rides, int bonus, int steps)
        {
            Rows = rows;
            Columns = columns;
            Fleet = fleet;
            Rides = rides;
            Bonus = bonus;
            Steps = steps;
        }
    }
}