namespace FcbUtils.BehaviorTree.Common
{
    public class Decorator : Node
    {
        protected Node Child;

        public Decorator(string name, Node child) : base(name)
        {
            Child = child;
        }
    }
}