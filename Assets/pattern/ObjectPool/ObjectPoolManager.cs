using System.Collections.Generic;
using System.Linq;
using pattern.Singleton;
using UnityEngine;

namespace DefaultNamespace
{
    public class ObjectPoolManager : Singleton<ObjectPoolManager>
    {
        [System.Serializable]
        public class PoolObjectInfo
        {
            public GameObject prefab;
            public int size;
            public GameObject parent;
        }

        public List<PoolObjectInfo> pools;
        public Dictionary<string, Queue<GameObject>> poolDictionary;

        private void Start()
        {
            poolDictionary = new();

            foreach (var pool in pools)
            {
                Queue<GameObject> poolQueue = new();

                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.prefab, pool.parent.transform);
                    obj.SetActive(false);
                    poolQueue.Enqueue(obj);
                }
                poolDictionary.Add(pool.prefab.tag, poolQueue);
            }
        }

        public GameObject SpawnObject(string objectTag, Vector3 position, Quaternion rotation)
        {
            // TODO : 딕셔너리에 태그가 없으면 null 반환 중 / 프리팹 없어도 만들어서 반환?
            if (!poolDictionary.ContainsKey(objectTag))
                return null;
            
            Queue<GameObject> poolQueue = poolDictionary[objectTag];
            GameObject objToSpawn = null;

            var count = poolQueue.Count;
            for (int i = 0; i < count; i++)
            {
                GameObject obj = poolQueue.Dequeue();
                poolQueue.Enqueue(obj);

                if (!obj.activeInHierarchy)
                {
                    objToSpawn = obj;
                    break;
                }
            }

            if (objToSpawn is null)
            {
                var info = pools.FirstOrDefault(p => p.prefab.CompareTag(objectTag));
                
                objToSpawn = Instantiate(info.prefab);
                objToSpawn.SetActive(false);
                
                poolQueue.Enqueue(objToSpawn);
            }
            
            objToSpawn.transform.position = position;
            objToSpawn.transform.rotation = rotation;
            objToSpawn.SetActive(true);

            
            
            IPooledObject pooledObj = objToSpawn.GetComponent<IPooledObject>();
            pooledObj?.OnObjectSpawn();

            return objToSpawn;
        }

        public void ReturnObjectToPool(GameObject obj)
        {
            var info = pools.FirstOrDefault(p => p.prefab.CompareTag(obj.tag));
            obj.transform.SetParent(info.parent.transform);
        }
        
    }
}