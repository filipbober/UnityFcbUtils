using FcbUtils.BehaviorTree.Common;
using UnityEngine;

//using ZombieSplasher;

namespace Samples.BehaviorTree
{
    public class Context : BehaviorState
    {
        public GameObject Self;
        public GameObject Target;
        public Vector3 MoveTarget;
        public Mover Mover;
    }
}