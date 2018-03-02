namespace Rides.MCTS
{
    public interface INode<TAction> where TAction : IAction
    {
        TAction Action { get; }
    }
}