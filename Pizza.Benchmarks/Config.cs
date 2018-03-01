using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace Pizza.Benchmarks
{
    public class Config : ManualConfig
    {
        public Config()
        {
            Add(new Job(EnvMode.LegacyJitX64, EnvMode.Clr, RunMode.Short));
        }
    }
}