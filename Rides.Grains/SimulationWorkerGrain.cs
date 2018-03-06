using Orleans;
using Orleans.Concurrency;
using Rides.GrainInterfaces;

namespace Rides.Grains
{
    [StatelessWorker]
    public class SimulationWorkerGrain: Grain, ISimulationWorkerGrain
    {
        
    }
}