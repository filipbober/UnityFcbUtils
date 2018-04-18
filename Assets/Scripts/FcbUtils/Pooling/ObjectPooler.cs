using System.Collections.Generic;
using UnityEngine;

namespace FcbUtils.Pooling
{
    public sealed class ObjectPooler : MonoBehaviour
    {

        public static ObjectPooler CurrentInstance;

        public GameObject PooledObject { get { return _pooledObject; } set { _pooledObject = value; } }
        public int PooledAmount { get { return _pooledAmount; } set { _pooledAmount = value; } }
        public bool WillGrow { get { return _willGrow; } set { _willGrow = value; } }

        [SerializeField]
        private GameObject _pooledObject;

        [SerializeField]
        private int _pooledAmount = 20;

        [SerializeField]
        private bool _willGrow = true;


        private List<GameObject> _pooledObjects;

        public GameObject GetPooledObject()
        {
            if (_pooledObjects == null)
                InitializePool();

            // Find inactive pool object and return it. 
            // Setting that object to active is not the responsibility of this method.
            for (int i = 0; i < _pooledObjects.Count; i++)
            {
                if (!_pooledObjects[i].activeInHierarchy)
                {
                    return _pooledObjects[i];
                }
            }

            // Expand the collection if it's too small and growing is permitted.
            if (_willGrow)
            {
                var obj = Instantiate(_pooledObject);
                obj.transform.SetParent(transform);
                _pooledObjects.Add(obj);
                return obj;
            }

            // We are out of object and we are not allowed to create new ones.
            return null;
        }

        private void Awake()
        {
            CurrentInstance = this;
        }

        private void Start()
        {
            if (_pooledObjects == null)
                InitializePool();
        }

        private void InitializePool()
        {
            // Create a pool of objects.
            _pooledObjects = new List<GameObject>();
            for (int i = 0; i < _pooledAmount; i++)
            {
                var obj = Instantiate(_pooledObject);
                obj.transform.SetParent(transform);
                _pooledObjects.Add(obj);
                obj.SetActive(false);
            }
        }

    }
}
