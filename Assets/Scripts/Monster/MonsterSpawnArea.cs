using System.Collections.Generic;
using UnityEngine;

namespace Monster
{
    public class MonsterSpawnArea : MonoBehaviour
    {
        
        [SerializeField]
        private List<MonsterSpawner> spawners = new List<MonsterSpawner>();

        public void SpawnMonsters()
        {
            foreach(var spawner in spawners){
                if (spawner is not null && spawner.gameObject.activeSelf)
                    spawner.ActivateSpawner();
            }
        }

        public void DespawnMonsters()
        {
            foreach(var spawner in spawners){
                if (spawner is not null) 
                    spawner.DeactivateSpawner();
            }
        }

    }
}