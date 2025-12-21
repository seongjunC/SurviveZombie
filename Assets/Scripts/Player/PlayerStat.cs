using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerStat : MonoBehaviour
    {
        [Header("이동 설정")] 
        public float moveSpeed = 5f;
        public float turnSpeed = 30f;
        public float CameraTurnSpeed = 10f;
        public float aimMoveSpeed = 2.5f;
        public float aimTurnSpeed = 0.5f;
        public float dashSpeed = 15.0f;
        
        [Header("전투 설정")]
        public int currentHealth;
        public int maxHealth = 100;
        public float reloadDuration = 2.0f;
        public int maxMagSize = 30;
        public int curMagSize;

        public bool isInvincible = false;
        public float dashDuration = 0.5f;

        private Dictionary<PlayerStatusType, float> statusMap = new Dictionary<PlayerStatusType, float>(); 
        public void Awake()
        {
            currentHealth = maxHealth;
            curMagSize = maxMagSize;

            statusMap.TryAdd(PlayerStatusType.moveSpeed, moveSpeed);
            statusMap.TryAdd(PlayerStatusType.turnSpeed, turnSpeed);
            statusMap.TryAdd(PlayerStatusType.reloadDuration, reloadDuration);
            statusMap.TryAdd(PlayerStatusType.aimMoveSpeed, aimMoveSpeed);
            statusMap.TryAdd(PlayerStatusType.aimTurnSpeed, aimTurnSpeed);
            statusMap.TryAdd(PlayerStatusType.dashSpeed, dashSpeed);
            statusMap.TryAdd(PlayerStatusType.dashDuration, dashDuration);
            statusMap.TryAdd(PlayerStatusType.maxHealth, maxHealth);
            statusMap.TryAdd(PlayerStatusType.currentHealth, currentHealth);
            statusMap.TryAdd(PlayerStatusType.curMagSize, curMagSize);

        }

        public bool ApplyDamage(int damage)
        {
            if (currentHealth <= 0 || isInvincible) return false;
            
            currentHealth -= Mathf.Max(0, currentHealth - damage);
            
            return currentHealth == 0;
        }

        public void ReduceMag()
        {
            curMagSize--;
        }

        public void Reload()
        {
            curMagSize = maxMagSize;
        }
        
        public float GetTypeValue(PlayerStatusType type)
        {
            return statusMap[type];
        }
        
        public void ApplyInvincible()
        {
            isInvincible = true;
        }

        public void RemoveInvincible()
        {
            isInvincible = false;
        }
        
        public void ApplyInvincible(float duration)
        {
            ApplyInvincible();
            
            Invoke(nameof(RemoveInvincible), duration);
        }
    }
}