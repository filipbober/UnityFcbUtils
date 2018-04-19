using FcbUtils.BehaviorTree.Common;

namespace FcbUtils.BehaviorTree.Composites
{
    /// <summary>
    /// AND
    /// </summary>
    public class Sequence : Composite
    {
        private int _currentChild;

        public Sequence(string name, params Node[] nodes) : base(name, nodes)
        {
        }

        public override NodeStatus OnBehave(BehaviorState state)
        {
            while (_currentChild < Children.Count)
            {
                var status = Children[_currentChild].Behave(state);

                // If child fails or keeps running, do the same
                if (status != NodeStatus.Success)
                {
                    return status;
                }

                _currentChild++;
            }

            return NodeStatus.Success;
        }

        public override void OnEnter(BehaviorState state)
        {
            _currentChild = 0;
        }
    }
}