namespace Rides.IndependentSet
{
    public class Edge
    {
        public Edge(Vertex source, Vertex target)
        {
            Source = source;
            Target = target;
        }

        public Vertex Source { get; }
        public Vertex Target { get; }

        public override string ToString()
        {
            return $"Edge: {nameof(Source)}: {Source} \n {nameof(Target)}: {Target}";
        }
    }
}