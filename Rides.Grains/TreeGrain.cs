using System;
using System.Threading.Tasks;
using Orleans;
using Rides.GrainInterfaces;

namespace Rides.Grains
{
    public class TreeGrain<TAction> : Grain, ITreeGrain<TAction> where TAction : IAction
    {
        private INodeGrain<TAction> _root;

        public async Task Init(IState<TAction> initialState)
        {
            _root = GrainFactory.GetGrain<INodeGrain<TAction>>(Guid.NewGuid());
            await _root.Init(Guid.Empty, initialState);
        }

        public Task<INodeView<TAction>> GetTopAction()
        {
            return _root.GetTopAction();
        }

        public Task Build()
        {
            return _root.Build();
        }
    }
}