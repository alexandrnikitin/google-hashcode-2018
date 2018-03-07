using System;
using Rides.GrainInterfaces;

namespace Rides.Grains
{
    [Serializable]
    public class NodeView<TAction> : INodeView<TAction> where TAction : IAction
    {
        public Guid Id { get; set; }
        public TAction Action { get; set; }
        public bool IsFinished { get; set; }
        public double Score { get; set; }
    }
}