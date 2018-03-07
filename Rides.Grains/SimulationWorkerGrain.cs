using System.Linq;
using System.Threading.Tasks;
using Orleans;
using Orleans.Concurrency;
using Rides.GrainInterfaces;

namespace Rides.Grains
{
    [StatelessWorker]
    public class SimulationWorkerGrain<TAction> : Grain, ISimulationWorkerGrain<TAction> where TAction : IAction
    {
        private readonly int _simulationSteps = 20;

        public async Task<double> Simulate(IState<TAction> state)
        {
            var counter = 0;
            while (state.GetAvailableActions().Any() && counter < _simulationSteps)
            {
                state = state.Apply(state.GetAvailableActions().First());
                counter++;
            }

            return state.GetScore();
        }
    }
}