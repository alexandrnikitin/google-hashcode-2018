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
            UntriedActions = new HashSet<TAction>(state.GetAvailableActions());
        }

        public Node<TAction> Parent { get; }
        public IState<TAction> State { get; }
        public TAction Action { get; }
        public IList<Node<TAction>> Children { get; } = new List<Node<TAction>>();
        public ISet<TAction> UntriedActions { get; }
        public double Score { get; set; }
        public bool Finished { get; set; }

        private readonly Random _random = new Random();
        private readonly int _epsilonExpansion = 50;
        private readonly int _epsilonExploration = 50;
        private readonly int _simulationSteps = 10;

        public void BuildTree(Func<int, long, bool> shouldContinue)
        {
            var iterations = 0;
            var timer = Stopwatch.StartNew();
            while (shouldContinue(iterations++, timer.ElapsedMilliseconds))
            {
                //Trace.WriteLine("Start!!!!!!!!!!!!!");
                var current = this;
                
                // nothing to expand and all nodes explored
                if (!current.UntriedActions.Any() && current.Children.All(x => x.Finished)) return;
                if (current.Finished) return;

                //Trace.WriteLine($"Selecting");
                // select
                if (TrySelect(out var next)) current = next;
                else
                {
                    //Trace.WriteLine($"Finishing node");
                    while (next != null)
                    {
                        if (next.Children.All(x => x.Finished) && !next.UntriedActions.Any())
                        {
                            next.Finished = true;
                        }

                        next = next.Parent;
                    }
                    continue;
                }

                //Trace.WriteLine($"Selected node with Score: {current.Score} Action: {current.Action}");
                var currentScore = 0.0;
                
                // expand
                if (current.UntriedActions.Any())
                {
                    current = current.ExpandRandom();
                    currentScore = current.State.GetScore();
                    //Trace.WriteLine($"Expanded to node {current.Action}");
                }

                
                // simulate
                if (current.TrySimulateRandom(out var simulatedState))
                {
                    currentScore = simulatedState.GetScore();
                    //Trace.WriteLine($"Simulated score {currentScore}");

                }

                if (currentScore>0)
                {
                    while (current != null)
                    {
                        current.Score = Math.Max(current.Score, currentScore);
                        current = current.Parent;
                    }
                }
            }
            //Trace.WriteLine($"iterations: {iterations}");
        }


        private bool TrySelect(out Node<TAction> next)
        {
            next = this;
            if (Children.All(x => x.Finished) && !UntriedActions.Any()) return false; //finish
            if (Children.All(x => x.Finished) && UntriedActions.Any()) return true; //expand current

            var isExpansion = _random.Next(100) < _epsilonExpansion;
            if (isExpansion && UntriedActions.Any())
            {
                return true;
            }

            var isExploration = _random.Next(100) < _epsilonExploration;
            if (isExploration)
            {
                return Children.Where(x => !x.Finished).ToList().RandomChoice().TrySelect(out next);
            }
            return Children.Where(x => !x.Finished).MaxElementBy(e => e.Score).TrySelect(out next);
        }


        private Node<TAction> ExpandRandom()
        {
            var action = UntriedActions.RandomChoice();
            UntriedActions.Remove(action);
            var state = (IState<TAction>)State.Clone();
            state.ApplyAction(action);
            var child = new Node<TAction>(state, action, this);
            Children.Add(child);
            return child;
        }

        private bool TrySimulateGreedy(out IState<TAction> state)
        {
            state = default(IState<TAction>);
            var availableActions = State.GetAvailableActions();
            if (!availableActions.Any()) return false;

            state = (IState<TAction>) State.Clone();
            var counter = 0;
            
            while (availableActions.Any() && counter < _simulationSteps)
            {
                state.ApplyAction(availableActions.First());
                availableActions = state.GetAvailableActions();
                counter++;
            }

            return true;
        }

        private bool TrySimulateRandom(out IState<TAction> state)
        {
            state = default(IState<TAction>);
            var availableActions = State.GetAvailableActions();
            if (!availableActions.Any()) return false;

            state = (IState<TAction>) State.Clone();
            var counter = 0;
            
            while (availableActions.Any() && counter < _simulationSteps)
            {
                state.ApplyAction(availableActions.RandomChoice());
                availableActions = state.GetAvailableActions();
                counter++;
            }

            return true;
        }


    }
}