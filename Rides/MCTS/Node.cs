using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Rides.MCTS
{
    public class Node<TAction> : INode<TAction> where TAction : IAction
    {
        public Node(IState<TAction> state, TAction action, Node<TAction> parent)
        {
            Parent = parent;
            State = state;
            Action = action;
            UntriedActions = new HashSet<TAction>(state.GetAvailableActions().Take(100));
        }

        public Node<TAction> Parent { get; }
        public IState<TAction> State { get; }
        public TAction Action { get; }
        public IList<Node<TAction>> Children { get; } = new List<Node<TAction>>();
        public ISet<TAction> UntriedActions { get; }
        public double Score { get; set; }

        private readonly Random _random = new Random();

        public void BuildTree(Func<int, long, bool> shouldContinue)
        {
            var iterations = 0;
            var timer = Stopwatch.StartNew();
            while (shouldContinue(iterations++, timer.ElapsedMilliseconds))
            {
                var current = this;

                // select
                while (!current.UntriedActions.Any() && current.Children.Any())
                {
                    var next = current.Select();
                    if (next == null) break;

                    current = next;
                }


                // expand
                if (current.UntriedActions.Any())
                {
                    current = current.ExpandGreedy();
                }

                // simulate
                var simulatedState = current.Simulate();

                // backprop
                if (simulatedState != null)
                {
                    var currentScore = simulatedState.GetScore();
                    while (current != null)
                    {
                        current.Score += currentScore;
                        current = current.Parent;
                    }
                }

            }
        }

        private Node<TAction> Select()
        {
            return _random.Next(100) < 20 ? Children.RandomChoice() : Children.MaxElementBy(e => e.Score);
        }

        private Node<TAction> ExpandGreedy()
        {
            var action = UntriedActions.RandomChoice();
            UntriedActions.Remove(action);
            var state = (IState<TAction>)State.Clone();
            state.ApplyAction(action);
            var child = new Node<TAction>(state, action, this);
            Children.Add(child);
            return child;
        }

        private IState<TAction> Simulate()
        {
            var availableActions = State.GetAvailableActions().Take(100).ToList();
            if (!availableActions.Any()) return null;

            var state = (IState<TAction>)State.Clone();
            var counter = 0;
            while (availableActions.Any() && counter < 10)
            {
                state.ApplyAction(availableActions.RandomChoice());
                availableActions = state.GetAvailableActions().Take(100).ToList();
                counter++;
            }

            return state;

        }


    }
}