using System.Threading.Tasks;
using Orleans;

namespace Rides.GrainInterfaces
{
    public interface ISimulationWorkerGrain<TAction> : IGrainWithIntegerKey where TAction : IAction
    {
        Task<double> Simulate(IState<TAction> state);
    }
}