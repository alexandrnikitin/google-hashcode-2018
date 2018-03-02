using System.Collections.Generic;

namespace Pizza
{
    public class RidesBuilder
    {
        public static List<Ride> Build(string[] lines)
        {
            var rides = new List<Ride>(lines.Length);

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var pars = line.Split(' ');
                var a = int.Parse(pars[0]);
                var b = int.Parse(pars[1]);
                var x = int.Parse(pars[2]);
                var y = int.Parse(pars[3]);
                var s = int.Parse(pars[4]);
                var f = int.Parse(pars[5]);
                var ride = new Ride {Id = i, From = new Point(a, b), To = new Point(x, y), Start = s, Finish = f};
                rides.Add(ride);
            }

            return rides;
        }
    }
}