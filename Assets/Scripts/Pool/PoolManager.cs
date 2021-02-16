using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pool
{
    public class PoolManager : MonoBehaviour
    {
        [SerializeField] private ObjectInfo[] poolObjects;

        private Dictionary<String, ObjectPool> _pools;
        public static PoolManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            _pools = new Dictionary<string, ObjectPool>();
        
            foreach (ObjectInfo info in poolObjects)
            {
                var pool = new ObjectPool();
            
                FillingPool(pool, info);
            
                _pools.Add(info.objectTag, pool);
            }
        }

        private static void FillingPool(ObjectPool pool, ObjectInfo info)
        {
            for (int i = 0; i < info.minCount; i++)
            {
                var el = Instantiate(info.gameObject);
                pool.SetElement(el);
            }
        }

        public GameObject GetElementByTag(String tag)
        {
            if (!_pools.Keys.Contains(tag)) return null;
        
            var pool = _pools[tag];
            var el = pool.GetElement();

            el = el ? el : Instantiate(GetObjectByTag(tag));
        
            return el;
        }

        public void SetElementByTag(String tag, GameObject element)
        {
            if (_pools.Keys.Contains(tag))
                _pools[tag].SetElement(element);
        }

        private GameObject GetObjectByTag(String tag)
        {
            foreach (var info in poolObjects)
            {
                if (info.objectTag.Equals(tag)) return info.gameObject;
            }

            return null;
        }
    
        [Serializable]
        public struct ObjectInfo
        {
            public GameObject gameObject;
            public String objectTag;
            public int minCount;
        }
    }
}
