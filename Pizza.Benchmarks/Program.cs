using BenchmarkDotNet.Running;

namespace Pizza.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<SampleBenchmarks>();
        }
    }
}