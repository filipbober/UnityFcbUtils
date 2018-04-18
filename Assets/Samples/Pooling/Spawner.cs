using System.Collections;
using UnityEngine;

namespace FcbUtils.Pooling.Sample
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private ObjectPooler _pooler;
        [SerializeField] private GameObject _objForPool;

        private void Start()
        {
            StartCoroutine(SpawnFromObjectPooler());

            Pool.Instance.CreatePool(_objForPool, false, 3);
            StartCoroutine(SpawnFromPool());
        }

        private IEnumerator SpawnFromObjectPooler()
        {
            while (true)
            {                
                var obj = _pooler.GetPooledObject();
                obj.transform.position = transform.position;
                obj.transform.rotation = Quaternion.identity;
                obj.SetActive(true);

                yield return new WaitForSeconds(1);
            }
        }

        private IEnumerator SpawnFromPool()
        {
            while (true)
            {
                Pool.Instance.SpawnObject(_objForPool, transform.position, Quaternion.identity);

                yield return new WaitForSeconds(1);
            }
        }
    }
}
