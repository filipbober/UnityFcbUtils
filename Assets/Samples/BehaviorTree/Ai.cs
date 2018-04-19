using FcbUtils.BehaviorTree.Common;
using FcbUtils.BehaviorTree.Composites;
using UnityEngine;

namespace Samples.BehaviorTree
{
    [RequireComponent(typeof(Mover))]
    public class Ai : MonoBehaviour
    {
        public GameObject Target;

        private Node _behaviorTree;
        Context _state;

        private void Start()
        {
            _behaviorTree = CreateBehaviorTree();
            _state = new Context
            {
                Self = gameObject,
                Target = Target,
                Mover = GetComponent<Mover>()
            };
        }

        private void FixedUpdate()
        {
            _behaviorTree.Behave(_state);
        }

        private static Node CreateBehaviorTree()
        {
            var selector = new Selector("test selector", new Move("test move", true));

            return selector;
        }
    }
}