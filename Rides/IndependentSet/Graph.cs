using System.Collections.Generic;

namespace Rides.IndependentSet
{
    public class Graph
    {
        public static Graph Build(List<Ride> rides)
        {
            var graph = new Graph();
            foreach (var ride in rides)
            {
                var vertex = new Vertex(ride);
                graph.Add(vertex);
            }

            foreach (var source in graph.Vertices)
            {
                foreach (var target in graph.Vertices)
                {
                    if (source == target) continue;

                    var rideDistance = source.Ride.From.Distance(source.Ride.To);
                    var pickupDistance = source.Ride.To.Distance(target.Ride.From);

                    // intersect
                    if (source.Ride.Start + rideDistance + pickupDistance >=
                        target.Ride.Finish - target.Ride.From.Distance(target.Ride.To))
                    {
                        var edge = new Edge(source, target);
                        graph.Edges.Add(edge);
                    }
                }
            }

            return graph;
        }

        public void Add(Vertex vertex)
        {
            Vertices.Add(vertex);
            VerticesSorted.Add(vertex);
        }

        public SortedSet<Vertex> VerticesSorted { get; set; } = new SortedSet<Vertex>(Vertex.FinishComparer);
        public List<Vertex> Vertices { get; set; } = new List<Vertex>();
        public List<Edge> Edges { get; set; } = new List<Edge>();

        public Vertex GetEarliestFinishRide(Vertex min)
        {
            return VerticesSorted.GetViewBetween(min, VerticesSorted.Max).Min;
        }

        public Vertex GetEarliestFinishRide()
        {
            return VerticesSorted.Min;
        }
    }
}