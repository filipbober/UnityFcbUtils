using UnityEngine;

namespace Samples.BehaviorTree
{
    public class Mover : MonoBehaviour
    {
        private Vector3 _dir = Vector3.zero;
        private float _speed;

        private void Update()
        {
            var planeDir = new Vector3(_dir.x, 0f, _dir.z);
            gameObject.transform.Translate(planeDir * _speed * Time.deltaTime);
        }

        public void SetDirection(Vector3 dir)
        {
            _dir = dir;
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }
    }
}