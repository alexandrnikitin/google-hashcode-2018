using System;
using System.Collections.Generic;

namespace Rides.MCTS
{
    public interface IState<TAction> : ICloneable
    {
        List<TAction> GetAvailableActions();
        void ApplyAction(TAction action);
        double GetScore();
    }
}