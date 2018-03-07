using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orleans;
using Rides.GrainInterfaces;

namespace Rides.Grains
{
    public class NodeGrain<TAction> : Grain, INodeGrain<TAction> where TAction:IAction
    {
        private List<INodeView<TAction>> _children;
        private bool _isFinished;
        private ISet<TAction> _untriedActions;
        private TAction _action;
        private IState<TAction> _state;
        private double _score;
        private Guid _parentId;

        private Random _random;
        private readonly int _epsilonExpansion = 70;
        private readonly int _epsilonExploration = 70;
        private readonly int _simulationSteps = 20;
        private readonly int _numberOfSimulations = 20;

        public override async Task OnActivateAsync()
        {
            _children = new List<INodeView<TAction>>();
            _untriedActions = new HashSet<TAction>();
            _random = new Random();
        }

        public Task Init(Guid parentId)
        {
            _parentId = parentId;
            return Task.CompletedTask;
        }

        public async Task Init(Guid parentId, IState<TAction> state)
        {
            _parentId = parentId;
            _state = state;
            foreach (var action in _state.GetAvailableActions())
            {
                _untriedActions.Add(action);
            }
        }

        public async Task Build()
        {
            if (_isFinished) return;
            if (TrySelect(out var next))
            {
                if (next != this.GetPrimaryKey())
                {
                    await GrainFactory.GetGrain<INodeGrain<TAction>>(next).Build();
                    return;
                }
            }
            else
            {
                _isFinished = true;
                if (_parentId != Guid.Empty)
                {
                    var nodeView = new NodeView<TAction> {Id = this.GetPrimaryKey(), IsFinished = _isFinished};
                    await GrainFactory.GetGrain<INodeGrain<TAction>>(_parentId).BackPropagate(nodeView);
                    return;
                }
            }

            // expand
            if (_untriedActions.Any())
            {
                var action = _untriedActions.RandomChoice();
                _untriedActions.Remove(action);

                var child = new NodeView<TAction>
                {
                    Id = Guid.NewGuid(),
                    Action = action
                };
                _children.Add(child);
                var toExpand = GrainFactory.GetGrain<INodeGrain<TAction>>(child.Id);
                await toExpand.Init(this.GetPrimaryKey(), _state);
                await toExpand.Expand(action);
            }
        }

        public async Task Expand(TAction action)
        {
            _action = action;
            _state = _state.Apply(action);

            var tasks = new Task<double>[_numberOfSimulations];
            for (var i = 0; i < _numberOfSimulations; i++)
            {
                var worker = GrainFactory.GetGrain<ISimulationWorkerGrain<TAction>>(0);
                tasks[i] = worker.Simulate(_state);
            }
            await Task.WhenAll(tasks);

            var simScore = tasks.Select(x => x.Result).Max();
            if (simScore > _score)
            {
                _score = simScore;
                if (_parentId != Guid.Empty)
                {
                    var nodeView = new NodeView<TAction> { Id = this.GetPrimaryKey(), Score = simScore};
                    await GrainFactory.GetGrain<INodeGrain<TAction>>(_parentId).BackPropagate(nodeView);
                }
            }
        }

        public async Task BackPropagate(INodeView<TAction> node)
        {
            var propagateFurther = false;
            var thisNode = _children.Single(x => x.Id == node.Id);
            if (node.Score > _score)
            {
                _score = node.Score;
                thisNode.Score = node.Score;
                propagateFurther = true;
            }

            if (node.IsFinished && !thisNode.IsFinished)
            {
                thisNode.IsFinished = true;
                if (_children.All(x => x.IsFinished) && !_untriedActions.Any())
                {
                    _isFinished = true;
                }

                propagateFurther = true;
            }

            if (propagateFurther && _parentId != Guid.Empty)
            {
                var newNode = new NodeView<TAction>
                {
                    Id = this.GetPrimaryKey(),
                    IsFinished = _isFinished,
                    Score = _score
                };

                await GrainFactory.GetGrain<INodeGrain<TAction>>(_parentId).BackPropagate(newNode);
            }
        }

        public Task<INodeView<TAction>> GetTopAction()
        {
            return Task.FromResult(_children.OrderByDescending(x => x.Score).FirstOrDefault());
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