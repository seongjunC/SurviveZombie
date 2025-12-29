using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerStat : MonoBehaviour
    {
        [Header("이동 설정")] public float moveSpeed = 5f;
        public float turnSpeed = 30f;
        public float aimMoveSpeed = 2.5f;
        public float aimTurnSpeed = 0.5f;
        public float dashSpeed = 15.0f;


        [Header("전투 설정")] 
        [SerializeField] private int _currentHealth;
        public int currentHealth
        {
            get => _currentHealth;
            private set
            {
                _currentHealth = value;
                
                if (currentHealth < 0) currentHealth = 0;
                
                if (statusEventMap.TryGetValue(PlayerStatusType.currentHealth, 
                        out var eventAction))
                {
                    eventAction?.Invoke(currentHealth);

                };
            }
        }
        
        public int maxHealth = 100;
        public float reloadDuration = 2.0f;
        public int maxMagSize = 30;
        [SerializeField] private int _curMagSize;

        public int curMagSize
        {
            get => _curMagSize;
            private set
            {
                if (value < 0) return;
                _curMagSize = value;
                
                if (statusEventMap.TryGetValue(PlayerStatusType.curMagSize, 
                        out var eventAction))
                {
                    eventAction?.Invoke(curMagSize);
                };
            }
        }

        private bool _isInvincible = false;

        public bool isInvincible
        {
            get => _isInvincible;
            private set
            {
                _isInvincible = value;
                if (statusEventMap.TryGetValue(PlayerStatusType.isInvincible, out var eventAction))
                {
                    eventAction?.Invoke(isInvincible? 1 : 0);
                }
            }
        }
        
        public float dashDuration = 0.5f;

        private Dictionary<PlayerStatusType, float> statusMap = new Dictionary<PlayerStatusType, float>(); 
        private Dictionary<PlayerStatusType, Action<int>> statusEventMap = new Dictionary<PlayerStatusType, Action<int>>();
        
        private Action<int> OnHealthChanged;
        private Action<int> OnMagSizeChanged;
        private Action<int> OnInvincibleChanged;
        public Action OnStatReady;
        public bool isStatReady;
        
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
            statusMap.TryAdd(PlayerStatusType.maxMagSize, maxMagSize);
            
            statusEventMap.TryAdd(PlayerStatusType.currentHealth, OnHealthChanged);
            statusEventMap.TryAdd(PlayerStatusType.curMagSize, OnMagSizeChanged);
            statusEventMap.TryAdd(PlayerStatusType.isInvincible, OnInvincibleChanged);
            
            isStatReady = true;
            OnStatReady?.Invoke();
        }

        public bool ApplyDamage(int damage)
        {
            if (currentHealth <= 0 || isInvincible) return false;
            
            currentHealth = Mathf.Max(0, currentHealth - damage);
            
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
        
        public void SubscribeEvent(PlayerStatusType type, Action<int> EventHandler)
        {
            if (statusEventMap.TryGetValue(type, out var eventAction))
            {
                eventAction += EventHandler;
                statusEventMap[type] = eventAction;
            }
        }

        public void UnsubscribeEvent(PlayerStatusType type, Action<int> EventHandler)
        {
            statusEventMap.TryGetValue(type, out var eventAction);
            
            if (eventAction is null) return;
            
            eventAction -= EventHandler;
            
            statusEventMap[type] = eventAction;
            
        }
    }
}