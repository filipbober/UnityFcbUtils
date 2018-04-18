using UnityEngine;

namespace FcbUtils.Pooling.Sample
{
    public class Mover : MonoBehaviour
    {
        public enum MoveDir
        {
            Forward,
            Right,
            Count
        }

        [SerializeField] private MoveDir _dir;
        [SerializeField] private float _speed = 2f;

        private void Update()
        {
            Vector3 dir;
            switch (_dir)
            {
                case MoveDir.Forward:
                    dir = Vector3.forward;
                    break;
                case MoveDir.Right:
                    dir = Vector3.right;
                    break;
                default:
                    dir = Vector3.zero;
                    break;
            }

            transform.Translate(dir * _speed * Time.deltaTime);
        }
    }
}
