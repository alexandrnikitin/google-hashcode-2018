using System;
using System.Threading.Tasks;
using Orleans;

namespace Rides.GrainInterfaces
{
    public interface INodeGrain<TAction> : IGrainWithGuidKey where TAction: IAction
    {
        Task Init(Guid parentId, IState<TAction> state);
        Task Build();
        Task Expand(TAction action);
        Task BackPropagate(INodeView<TAction> node);
        Task<INodeView<TAction>> GetTopAction();
    }
}