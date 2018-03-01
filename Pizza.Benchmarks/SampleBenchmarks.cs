using BenchmarkDotNet.Attributes;

namespace Pizza.Benchmarks
{
    [Config(typeof(Config))]
    [MemoryDiagnoser]
    public class SampleBenchmarks
    {

        [GlobalSetup]
        public void Setup()
        {
        }

        [Benchmark]
        public void Sample()
        {
        }
    }
}