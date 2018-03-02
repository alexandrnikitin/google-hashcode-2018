using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Pizza.MonteCarloTreeSearch
{
    public class MonteCarloTreeSearch
    {
        private class Node<TAction> : IMctsNode<TAction> where TAction: IAction
        {
            public Node(IState<TAction> state, TAction action = default(TAction), Node<TAction> parent = null)
            {
                Parent = parent;
                State = state;
                Action = action;
                UntriedActions = new HashSet<TAction>(state.GetAvailableActions());
            }

            public Node<TAction> Parent { get; }
            public IState<TAction> State { get; }
            public TAction Action { get; }
            public IList<Node<TAction>> Children { get; } = new List<Node<TAction>>();
            public ISet<TAction> UntriedActions { get; }

            public int NumRuns { get; set; }
            public double NumWins { get; set; }

            private static double c = Math.Sqrt(2);
            public double ExploitationValue => NumWins / NumRuns;
            public double ExplorationValue => (Math.Sqrt(2*Math.Log(Parent.NumRuns) / NumRuns));
            private double UCT => ExploitationValue + ExplorationValue;

            public void BuildTree(Func<int, long, bool> shouldContinue)
            {
                var iterations = 0;
                var timer = Stopwatch.StartNew();
                while (shouldContinue(iterations++, timer.ElapsedMilliseconds))
                {
                    var current = this;
                    while (!current.UntriedActions.Any() && current.Children.Any())
                    {
                        var next = current.Select();
                        if (next != null) current = next;
                        else break;
                    }

                    if (current.UntriedActions.Any())
                    {
                        current = current.Expand();
                    }

                    var simulatedState = current.Simulate();

                    if (simulatedState != null)
                    {
                        var currentScore = simulatedState.GetResult();
                        while (current != null)
                        {
                            current.NumRuns++;
                            current.NumWins += currentScore;
                            current = current.Parent;
                        }
                    }

                }
            }

            private IState<TAction> Simulate()
            {
                var availableActions = State.GetAvailableActions();
                if (!availableActions.Any()) return null;

                var state = State.Clone();
                var counter = 0;
                while (availableActions.Any() && counter < 10)
                {
                    state.ApplyAction(availableActions.RandomChoice());
                    availableActions = state.GetAvailableActions();
                    counter++;
                }

                return state;

            }

            private Node<TAction> Expand()
            {
                var action = UntriedActions.RandomChoice();
                UntriedActions.Remove(action);
                var state = State.Clone();
                state.ApplyAction(action);
                var child = new Node<TAction>(state, action, this);
                Children.Add(child);
                return child;
            }

            private static Random _random = new Random();
            private Node<TAction> Select()
            {
                if (_random.Next(100) < 50)
                {
                    return Children.RandomChoice();
                } 
                return Children.MaxElementBy(e => e.NumWins);
            }

            public override string ToString()
            {
                return $"{nameof(NumWins)}: {NumWins}, {nameof(NumRuns)}: {NumRuns}, {nameof(ExploitationValue)}: {ExploitationValue}, {nameof(ExplorationValue)}: {ExplorationValue}, {nameof(UCT)}: {UCT}";
            }

//            public override string ToString()
//            {
//                return $"{NumWins}/{NumRuns}: ({ExploitationValue}/{ExplorationValue}={UCT})";
//            }
        }

        public static IEnumerable<IMctsNode<TAction>> GetTopActions<TAction>(IState<TAction> state, int maxIterations) where TAction : IAction
        {
            return GetTopActions(state, maxIterations, long.MaxValue);
        }

        public static IEnumerable<IMctsNode<TAction>> GetTopActions<TAction>(IState<TAction> state, long timeBudget) where TAction : IAction
        {
            return GetTopActions(state, int.MaxValue, timeBudget);
        }

        public static IEnumerable<IMctsNode<TAction>> GetTopActions<TAction>(IState<TAction> state, int maxIterations, long timeBudget) where TAction : IAction
        {
            var root = new Node<TAction>(state);
            root.BuildTree((numIterations, elapsedMs) => numIterations < maxIterations && elapsedMs < timeBudget);
            return root.Children
                .OrderByDescending(n => n.NumWins);
        }
    }
}
