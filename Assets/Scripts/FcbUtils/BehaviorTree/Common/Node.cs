using UnityEngine;

namespace FcbUtils.BehaviorTree.Common
{
    public abstract class Node : IBehavior
    {
        private readonly bool _showDebug = false;
        protected string Name { get; }

        private NodeStatus _status = NodeStatus.Invalid;

        public Node(string name, bool showDebug = false)
        {
            Name = name;
            _showDebug = showDebug;
        }

        public virtual NodeStatus Behave(BehaviorState state)
        {
            // TODO: Checking conditionals every tick hits performance
            if (_status != NodeStatus.Running)
            {
                OnEnter(state);
            }

            if (_showDebug)
            {
                Debug.Log("OnBehave " + Name);
            }

            _status = OnBehave(state);

            if (_status != NodeStatus.Running)
            {
                OnExit(state);
            }

            if (_showDebug)
            {
                Debug.Log("Return " + Name + " = " + _status.ToString());
            }

            return _status;
        }

        public virtual NodeStatus OnBehave(BehaviorState state)
        {
            Debug.LogWarning("Not implemented");
            return NodeStatus.Invalid;
        }

        public virtual void OnEnter(BehaviorState state)
        {
            if (_showDebug)
            {
                Debug.Log("OnEnter " + Name);
            }

            Debug.LogWarning("Not implemented");
        }

        public virtual void OnExit(BehaviorState state)
        {
            if (_showDebug)
            {
                Debug.Log("OnExit " + Name);
            }

            Debug.LogWarning("Not implemented");
        }
    }
}