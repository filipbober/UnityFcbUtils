namespace FcbUtils.BehaviorTree.Common
{
    public interface IBehavior
    {
        void OnEnter(BehaviorState state);
        NodeStatus OnBehave(BehaviorState state);
        void OnExit(BehaviorState state);
    }
}