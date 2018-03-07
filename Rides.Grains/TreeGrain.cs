using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orleans;
using Rides.GrainInterfaces;
using Rides.MCTS;

namespace Rides.Grains
{
    public class TreeGrain : Grain, ITreeGrain
    {
        private INodeGrain _root;

        public override Task OnActivateAsync()
        {
            return base.OnActivateAsync();
        }

        public async Task Init(IState<IAction> initialState)
        {
            
        }

        public async Task Build()
        {
        }

        public async Task<INode<IAction>> GetTopAction()
        {
            throw new NotImplementedException();
        }
    }

    public class NodeView
    {
        public Guid Id { get; set; }
        public IAction Action { get; set; }
        public bool IsFinished { get; set; }
        public double Score { get; set; }
    }

    public class NodeGrain : Grain, INodeGrain
    {
        private List<NodeView> _children;
        private bool _isFinished;
        private ISet<IAction> _untriedActions;
        private IState<IAction> _state; // TODO to init
        private double _score;
        private Guid _parent;

        private Random _random;
        private readonly int _epsilonExpansion = 70;
        private readonly int _epsilonExploration = 70;
        private readonly int _simulationSteps = 20;

        public override async Task OnActivateAsync()
        {
            _children = new List<NodeView>();
            _untriedActions = new HashSet<IAction>();
            _isFinished = false;
            _random = new Random();
        }

        public async Task Build()
        {
            if (_isFinished) return;
            if (TrySelect(out var next))
            {
                if (next != this.GetPrimaryKey())
                {
                    await GrainFactory.GetGrain<INodeGrain>(next).Build();
                    return;
                }
            }
            else
            {
                // backprop isfinished
            }

            // expand
            if (_untriedActions.Any())
            {
                var action = _untriedActions.RandomChoice();
                _untriedActions.Remove(action);

                var child = new NodeView
                {
                    Id = Guid.NewGuid(),
                    Action = action
                };
                _children.Add(child);
                var toExpand = GrainFactory.GetGrain<INodeGrain>(child.Id);
                // tODO init state and parent
                await toExpand.Expand(action);
            }
            //TODO currentScore = current.State.GetScore();
        }

        public async Task Expand(IAction action)
        {
            _state = _state.Apply(action);
            var availableActions = _state.GetAvailableActions();
            if (!availableActions.Any()) return;

            // TODO more simulations in parallel
            var counter = 0;
            var simState = _state;
            while (availableActions.Any() && counter < _simulationSteps)
            {
                simState = simState.Apply(availableActions.First());
                availableActions = simState.GetAvailableActions();
                counter++;
            }

            var simScore = simState.GetScore();
            if (simScore > _score)
            {
                _score = simScore;
                if (_parent != Guid.Empty)
                {
                    await GrainFactory.GetGrain<INodeGrain>(_parent).BackPropagate(simScore);
                }
            }
        }

        public async Task BackPropagate(double score)
        {
            if (score > _score)
            {
                _score = score;
                if (_parent != Guid.Empty)
                {
                    await GrainFactory.GetGrain<INodeGrain>(_parent).BackPropagate(score);
                }
            }
        }

        private bool TrySelect(out Guid next)
        {
            if (_children.All(x => x.IsFinished) && !_untriedActions.Any())
            {
                // TODO throw?
                next = default;
                return false;
            }

            if (_children.All(x => x.IsFinished) && _untriedActions.Any())
            {
                next = this.GetPrimaryKey();
                return true; //expand current
            }

            var isExpansion = _random.Next(100) < _epsilonExpansion;
            if (isExpansion && _untriedActions.Any())
            {
                next = this.GetPrimaryKey();
                return true;
            }

            var isExploration = _random.Next(100) < _epsilonExploration;
            if (isExploration)
            {
                next = _children.Where(x => !x.IsFinished).ToList().RandomChoice().Id;
                return true;
            }
            next = _children.Where(x => !x.IsFinished).MaxElementBy(e => e.Score).Id;
            return true;
        }

    }
}