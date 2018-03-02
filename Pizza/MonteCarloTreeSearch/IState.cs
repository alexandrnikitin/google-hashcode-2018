using System.Collections.Generic;

namespace Pizza.MonteCarloTreeSearch
{
    public interface IState<TAction>
    {
        IState<TAction> Clone();

        HashSet<TAction> GetAvailableActions();

        void ApplyAction(TAction action);

        double GetResult();
    }
}