using DefaultNamespace;
using UnityEngine;

namespace Monster
{
    public class MonsterSpawner : MonoBehaviour
    {
        [Header("Settings")] public string monsterTag = "Enemy";
        
        private GameObject monster;
        private int savedHp = 0;
        private bool _isDead = false;

        public void ActivateSpawner()
        {
            if (_isDead) return;
            
            monster ??= ObjectPoolManager.Instance.SpawnObject(
                monsterTag, transform.position, Quaternion.identity);

            if (monster is null) return;
            
            monster.transform.SetParent(this.transform);

            var controller = monster.GetComponent<MonsterController>();

            if (controller is not null)
            {
                controller.startPos = transform.position;
                controller.OnDeath -= HandleMonsterDeath;
                controller.OnDeath += HandleMonsterDeath;
                    
                if (savedHp > 0) controller.currentHp = savedHp;
            }
        }

        public void DeactivateSpawner()
        {
            if (_isDead) return;
            
            MonsterDespawn(true);
        }
        
        private void HandleMonsterDeath()
        {
            if (_isDead) return;
            
            _isDead = true;

            MonsterDespawn(false);
            
            gameObject.SetActive(false);
        }

        private void MonsterDespawn(bool isAlive)
        {
            if (monster is null) return;
            
            var controller = monster.GetComponent<MonsterController>();
            
            if (isAlive)
            {
                savedHp = controller.currentHp;
                monster.SetActive(false);
            }
            else savedHp = 0;
                        
            ObjectPoolManager.Instance.ReturnObjectToPool(monster);
            controller.OnDeath -= HandleMonsterDeath;
            
            monster = null;
        }
        

    }
}