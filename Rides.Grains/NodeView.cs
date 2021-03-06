﻿using System;
using Orleans.Concurrency;
using Rides.GrainInterfaces;

namespace Rides.Grains
{
    [Serializable]
    [Immutable]
    public class NodeView<TAction> : INodeView<TAction> where TAction : IAction
    {
        public NodeView(Guid id, TAction action, bool isFinished, double score)
        {
            Id = id;
            Action = action;
            IsFinished = isFinished;
            Score = score;
        }

        public Guid Id { get; set; }
        public TAction Action { get; set; }
        public bool IsFinished { get; set; }
        public double Score { get; set; }
    }
}