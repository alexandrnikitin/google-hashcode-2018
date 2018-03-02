namespace Rides
{
    public struct Ride
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
    }
}