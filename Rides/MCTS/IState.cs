using System.Collections.Generic;

namespace Rides.MCTS
{
    public interface IState<TAction> where TAction : IAction
    {
        IEnumerable<TAction> GetAvailableActions();
        TAction GetRandomAction();
        IState<TAction> Apply(TAction action);
        double GetScore();
    }
}