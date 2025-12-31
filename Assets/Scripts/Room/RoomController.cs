using System;
using Monster;
using UnityEngine;

namespace Room
{
    [RequireComponent(typeof(MeshCollider))]
    public class RoomController : MonoBehaviour
    {
        public int roomNum;
        public int[] connectedRoomNums;
        public MonsterSpawnArea spawnArea;

        public void Awake()
        {
            RoomManager.Instance.RegisterRoom(this);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                RoomManager.Instance.EnterRoom(this);
            }
        }

        public void SpawnMonsters()
        {
            spawnArea?.SpawnMonsters();
        }

        public void DespawnMonsters()
        {
            spawnArea?.DespawnMonsters();
        }

        public void OnDestroy()
        {
            RoomManager.Instance.UnRegisterRoom(this);
        }
    }
}