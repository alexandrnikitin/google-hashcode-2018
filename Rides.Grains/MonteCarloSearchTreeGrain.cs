using System.Linq;
using System.Threading.Tasks;
using Orleans;
using Rides.GrainInterfaces;
using Rides.MCTS;

namespace Rides.Grains
{
    public class MonteCarloSearchTreeGrain<TAction> : Grain, IMonteCarloTreeSearchGrain<TAction> where TAction : IAction
    {
        private MonteCarloTreeSearch<TAction> _tree;

        public async Task Init(IState<TAction> initialState)
        {
            _tree = new MonteCarloTreeSearch<TAction>(initialState);
        }

        public Task<Node<TAction>> GetTopAction(int maxIterations, long timeBudget)
        {
            _tree.BuildTree((numIterations, elapsedMs) => numIterations < maxIterations && elapsedMs < timeBudget);
            var node = _tree.GetTopActions(10, 10).FirstOrDefault();
            return Task.FromResult(node);
        }
    }
}