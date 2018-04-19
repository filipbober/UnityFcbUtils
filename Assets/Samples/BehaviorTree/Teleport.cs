using UnityEngine;

namespace Samples.BehaviorTree
{
    public class Teleport : MonoBehaviour
    {
        private const float MaxDistance = 10f;

        private void Start()
        {
            InvokeRepeating(nameof(Blink), 2.0f, 2.0f);
        }

        private void Blink()
        {
            var rndPos = new Vector3(Random.Range(-MaxDistance, MaxDistance), 0f, Random.Range(-MaxDistance, MaxDistance));
            var currentPos = transform.position;
            transform.position = currentPos + rndPos;
        }
    }
}