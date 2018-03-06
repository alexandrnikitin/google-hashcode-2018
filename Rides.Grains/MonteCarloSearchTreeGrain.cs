using System.Linq;
using System.Threading.Tasks;
using Orleans;
using Rides.GrainInterfaces;
using Rides.MCTS;

namespace Rides.Grains
{
    public class MonteCarloSearchTreeGrain<TAction> : Grain, IMonteCarloTreeSearchGrain<TAction> where TAction : IAction
    {
        private DMCTS.Node<TAction> _tree;

        public async Task Init(IState<TAction> initialState)
        {

            _tree = new DMCTS.Node<TAction>(GrainFactory, initialState, default, default);
        }

        public async Task<INode<TAction>> GetTopAction(int maxIterations, long timeBudget)
        {
            await _tree.BuildTreeAsync((numIterations, elapsedMs) => numIterations < maxIterations && elapsedMs < timeBudget);
            var node = _tree.GetTopActions().FirstOrDefault();
            return node;
        }
    }
}