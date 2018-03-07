using System.Threading.Tasks;
using Orleans;
using Rides.MCTS;

namespace Rides.GrainInterfaces
{
    public interface ITreeGrain : IGrainWithGuidKey
    {
        Task Init(IState<IAction> initialState);
        //        Task<INode<TAction>> GetTopAction(int maxIterations, long timeBudget);
        Task Build();
    }
}