using System;
using System.Collections.Generic;

namespace Rides.MCTS
{
    public interface IState<TAction> : ICloneable
    {
        IEnumerable<TAction> GetAvailableActions();
        void ApplyAction(TAction action);
        double GetScore();
    }
}