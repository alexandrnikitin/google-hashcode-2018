using System;
using System.Threading.Tasks;
using Orleans;

namespace Rides.GrainInterfaces
{
    public interface ITreeGrain<TAction> : IGrainWithGuidKey where TAction : IAction
    {
        Task Init(IState<TAction> initialState);
        Task Build();
        Task<INodeView<TAction>> GetTopAction();
    }
}