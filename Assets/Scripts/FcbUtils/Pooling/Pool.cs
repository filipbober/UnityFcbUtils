using System.Collections.Generic;
using UnityEngine;

namespace FcbUtils.Pooling
{
    public sealed class Pool : MonoBehaviour
    {
        public static Pool Instance { get; private set; }

        private readonly Dictionary<int, PoolInstance> _poolDict = new Dictionary<int, PoolInstance>();

        private void Awake()
        {
            // Setup the singleton instance
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }


        public void CreatePool(GameObject prefab, bool willGrow = true, int size = 0)
        {
            if (willGrow == false && size < 1)
            {
                Debug.LogError("Pool is set to constant size while size is less than one. No object will be spawned");
                return;
            }

            var key = prefab.GetInstanceID();

            if (!_poolDict.ContainsKey(key))
            {
                var newInstance = new PoolInstance(transform, prefab, willGrow);
                _poolDict.Add(key, newInstance);
            }

            while (size > _poolDict[key].Pool.Count)
            {
                var newObj = CreateObjectInPool(key, prefab);
                SetParent(newObj, _poolDict[key].Holder);
            }
        }

        public GameObject SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            var key = prefab.GetInstanceID();

            if (!_poolDict.ContainsKey(key))
            {
                Debug.LogError("There is no pool for the specified prefab");
                return null;
            }

            foreach (var poolObj in _poolDict[key].Pool)
            {
                if (!poolObj.activeInHierarchy)
                {
                    var obj = poolObj;

                    PrepareObject(obj, position, rotation);
                    return obj;
                }
            }

            if (_poolDict[key].WillGrow)
            {
                var newObj = CreateObjectInPool(key, prefab);
                SetParent(newObj, _poolDict[key].Holder);

                PrepareObject(newObj, position, rotation);
                return newObj;
            }

            Debug.LogWarning("Reusing existing object");
            var activeObj = _poolDict[key].Pool[Random.Range(0, _poolDict[key].Pool.Count)];
            PrepareObject(activeObj, position, rotation);
            return activeObj;
        }

        private void SetParent(GameObject obj, Transform parent)
        {
            obj.transform.parent = parent;
        }

        private GameObject CreateObjectInPool(int poolKey, GameObject prefab)
        {
            var newObj = Instantiate(prefab);
            newObj.SetActive(false);
            _poolDict[poolKey].Pool.Add(newObj);

            return newObj;
        }

        private static void PrepareObject(GameObject obj, Vector3 position, Quaternion rotation)
        {
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);
        }

    }

    public sealed class PoolInstance
    {
        public readonly GameObject Prefab;
        public readonly Transform Holder;
        public bool WillGrow;
        public List<GameObject> Pool = new List<GameObject>();

        public PoolInstance(Transform parent, GameObject prefab, bool willGrow)
        {
            Prefab = prefab;
            WillGrow = willGrow;

            Holder = new GameObject(prefab.name + " Pool").transform;
            Holder.parent = parent;
        }
    }
}
