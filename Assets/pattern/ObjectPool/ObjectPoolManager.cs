using System.Collections.Generic;
using pattern.Singleton;
using UnityEngine;

namespace DefaultNamespace
{
    public class ObjectPoolManager : Singleton<ObjectPoolManager>
    {
        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int size;
        }

        public List<Pool> pools;
        public Dictionary<string, Queue<GameObject>> poolDictionary;

        private void Start()
        {
            poolDictionary = new();

            foreach (var pool in pools)
            {
                Queue<GameObject> poolQueue = new();

                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.prefab);
                    obj.SetActive(false);
                    poolQueue.Enqueue(obj);
                }
                poolDictionary.Add(pool.tag, poolQueue);
            }
        }

        public GameObject SpawnObject(string tag, Vector3 position, Quaternion rotation)
        {
            if (!poolDictionary.ContainsKey(tag)) return null;
            
            GameObject objToSpawn = poolDictionary[tag].Dequeue();
            objToSpawn.SetActive(true);
            objToSpawn.transform.position = position;
            objToSpawn.transform.rotation = rotation;
            
            IPooledObject pooledObj = objToSpawn.GetComponent<IPooledObject>();
            if (pooledObj is not null)
            {
                pooledObj.OnObjectSpawn();
            }
            
            poolDictionary[tag].Enqueue(objToSpawn);
            return objToSpawn;
        }
        
    }
}