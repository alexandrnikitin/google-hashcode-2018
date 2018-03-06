using System.Threading.Tasks;
using Orleans;
using Rides.MCTS;

namespace Rides.GrainInterfaces
{
    public interface IMonteCarloTreeSearchGrain<TAction> : IGrainWithGuidKey where TAction : IAction
    {
        Task Init(IState<TAction> initialState);
        Task<INode<TAction>> GetTopAction(int maxIterations, long timeBudget);
    }
}