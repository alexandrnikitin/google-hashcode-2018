using System;
using System.Collections.Generic;

namespace Rides.MCTS
{
    public interface IState<TAction> : ICloneable where TAction: IAction
    {
        List<TAction> GetAvailableActions();
        void ApplyAction(TAction action);
        IState<TAction> Apply(TAction action);
        double GetScore();
    }
}