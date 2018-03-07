using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;
using Rides.MCTS;

namespace Rides.GrainInterfaces
{
    public interface INodeGrain : IGrainWithGuidKey
    {
        //        Task Build();
        //        Task<IEnumerable<INodeGrain>> GetChildren();
//        Task<bool> IsFinished { get; set; }
//        Task<bool> TrySelect(out Guid nodeId);
        Task Build();
        Task Expand(IAction action);
        Task BackPropagate(double score); // todo prop state
    }
}