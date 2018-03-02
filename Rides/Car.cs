namespace Rides
{
    public struct Car
    {
        public Car(int id, Point location, int time)
        {
            Id = id;
            Location = location;
            Time = time;
        }

        public int Id { get; }
        public Point Location { get; }
        public int Time { get; }

        public bool CanMakeIt(Ride ride)
        {
            return ride.Finish >= Time + Location.Distance(ride.From) + ride.From.Distance(ride.To);
        }
    }
}