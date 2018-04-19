using FcbUtils.BehaviorTree.Common;
using UnityEngine;

namespace Samples.BehaviorTree
{
    public class Move : Leaf
    {
        private int _ticks;
        private float _currentSpeed;

        public Move(string name, bool showDebug) : base(name, showDebug)
        {
        }

        public override NodeStatus OnBehave(BehaviorState state)
        {
            var context = (Context)state;

            if (context.Target == null)
                return NodeStatus.Failure;

            if (AtDestination(context))
                return NodeStatus.Success;

            //Move for 120 ticks(4s) max
            if (_ticks > 240)
            {
                return NodeStatus.Success;
            }

            _currentSpeed += 0.01f;
            context.Mover.SetSpeed(_currentSpeed);

            _ticks += 1;

            return NodeStatus.Running;
        }

        public override void OnEnter(BehaviorState state)
        {
            var context = (Context)state;

            _ticks = 0;
            _currentSpeed = 0.5f;
            context.Mover.SetDirection((context.Target.transform.position - context.Self.transform.position).normalized);
            context.Mover.SetSpeed(1f);
        }

        public override void OnExit(BehaviorState state)
        {
            var context = (Context)state;
            context.Mover.SetSpeed(0f);

            _ticks = 0;
        }

        private static bool AtDestination(Context context)
        {
            return !(Vector3.Distance(context.Self.transform.position, context.Target.transform.position) > 3f);
        }
    }
}