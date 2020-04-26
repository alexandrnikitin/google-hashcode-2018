using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Rides.MCTS
{
    public class Node<TAction> : INode<TAction> where TAction : IAction
    {
        public Node(IState<TAction> state, TAction action, Node<TAction> parent)
        {
            Parent = parent;
            State = state;
            Action = action;
            AvailableActions = state.GetAvailableActions().GetEnumerator();
            AvailableActions.MoveNext();
        }

        public IEnumerator<TAction> AvailableActions { get; set; }

        public Node<TAction> Parent { get; set; }
        public IState<TAction> State { get; }
        public TAction Action { get; }
        public IList<Node<TAction>> Children { get; } = new List<Node<TAction>>();
        public double Score { get; set; }
        public bool Finished { get; set; }

        private readonly Random _random = new Random();
        private readonly int _epsilonExpansion = 100;
        private readonly int _epsilonExploration = 100;
        private readonly int _simulationSteps = 10;
        private int _numberOfSimulations = 10;

        public void BuildTree(Func<int, long, bool> shouldContinue)
        {
            var iterations = 0;
            var timer = Stopwatch.StartNew();
            while (shouldContinue(iterations++, timer.ElapsedMilliseconds))
            {
                //Trace.WriteLine("Start!!!!!!!!!!!!!");
                var current = this;
                
                // nothing to expand and all nodes explored
                if (current.Finished) return;

                //Trace.WriteLine($"Selecting");
                // select
                if (TrySelect(out var next)) current = next;
                else
                {
                    Finished = true;
                    if (Parent!=null)
                    {
                        next = Parent;
                        while (next != null)
                        {
                            if (next.Children.All(x => x.Finished) && AvailableActions.Current == null)
                            {
                                next.Finished = true;
                            }
                            next = next.Parent;
                        }
                    }
                    continue;
                    //Trace.WriteLine($"Finishing node");
                }

                //Trace.WriteLine($"Selected node with Score: {current.Score} Action: {current.Action}");
                
                // expand
                if (AvailableActions.Current != null)
                {
                    current = current.ExpandRandom();
                    //Trace.WriteLine($"Expanded to node {current.Action}");
                }


                // simulate
                //if (current.TrySimulateRandom(out var simulatedState))
                var simScore = TrySimulateRandomMany();
                {
                    //Trace.WriteLine($"Simulated score {currentScore}");
                }

                //var simScore = simulatedState.GetScore();
                if (simScore > current.Score)
                {
                    while (current != null)
                    {
                        current.Score = Math.Max(current.Score, simScore);
                        current = current.Parent;
                    }
                }
            }
            //Trace.WriteLine($"iterations: {iterations}");
        }


        private bool TrySelect(out Node<TAction> next)
        {
            next = this;
            if (Children.All(x => x.Finished) && AvailableActions.Current == null) return false; //finish
            if (Children.All(x => x.Finished) && AvailableActions.Current != null) return true; //expand current

            var isExpansion = _random.Next(100) < _epsilonExpansion;
            if (isExpansion && AvailableActions.Current != null)
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
            var action = AvailableActions.Current;
            AvailableActions.MoveNext();
            var state = State.Apply(action);
            var child = new Node<TAction>(state, action, this);
            Children.Add(child);
            return child;
        }



        private double TrySimulateRandomMany()
        {
            var tasks = new Task<double>[_numberOfSimulations];
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    var state = State;
                    var randomAction = state.GetRandomAction();
                    if (randomAction==null) return state.GetScore();

                    var counter = 0;

                    while (randomAction != null && counter < _simulationSteps)
                    {
                        state = state.Apply(randomAction);
                        randomAction = state.GetRandomAction();
                        counter++;
                    }

                    return state.GetScore();

                });
            }

            Task.WaitAll(tasks);
            return tasks.Max(x => x.Result);

        }


    }
}