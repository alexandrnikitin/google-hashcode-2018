using System;
using System.Collections.Generic;
using System.Linq;

namespace Rides.MCTS
{
    public class MonteCarloTreeSearch<TAction> where TAction : IAction
    {
        private readonly Node<TAction> _root;

        public MonteCarloTreeSearch(IState<TAction> initialState)
        {
            _root = new Node<TAction>(initialState, default(TAction), null);
        }

        public void BuildTree(Func<int, long, bool> shouldContinue)
        {
            _root.BuildTree(shouldContinue);
        }

        public IEnumerable<Node<TAction>> GetTopActions(int maxIterations, long timeBudget)
        {
            _root.BuildTree((numIterations, elapsedMs) => numIterations < maxIterations && elapsedMs < timeBudget);
            return _root.Children.OrderByDescending(x => x.Score);
        }

        public static Node<TAction> Create<TAction>(IState<TAction> state) where TAction : IAction
        {
            return new Node<TAction>(state, default(TAction), null);
        }

        public static IEnumerable<INode<TAction>> GetTopActions<TAction>(IState<TAction> state, int maxIterations) where TAction : IAction
        {
            return GetTopActions(state, maxIterations, long.MaxValue);
        }

        public static IEnumerable<INode<TAction>> GetTopActions<TAction>(IState<TAction> state, long timeBudget) where TAction : IAction
        {
            return GetTopActions(state, int.MaxValue, timeBudget);
        }

        public static IEnumerable<INode<TAction>> GetTopActions<TAction>(IState<TAction> state, int maxIterations, long timeBudget) where TAction : IAction
        {
            var root = Create(state);
            root.BuildTree((numIterations, elapsedMs) => numIterations < maxIterations && elapsedMs < timeBudget);
            return root.Children.OrderByDescending(x => x.Score);
        }

        public static IEnumerable<Node<TAction>> GetTopActions<TAction>(Node<TAction> node, int maxIterations, long timeBudget) where TAction : IAction
        {
            node.BuildTree((numIterations, elapsedMs) => numIterations < maxIterations && elapsedMs < timeBudget);
            return node.Children.OrderByDescending(x => x.Score);
        }
    }
}
