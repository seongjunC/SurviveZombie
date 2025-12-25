using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("플레이어 스탯")]
        [SerializeField] private PlayerStat stat;

        
        [Header("카메라 설정")]
        public CinemachineCamera mainCamera;
        public CinemachineCamera aimCamera;
        public CinemachineBrain mainCameraComponent;
        
        public GameObject gunObject;
        public Transform SpawnedBullet;
        
        private float gravity = -9.81f;
        private float verticalVelocity;
        
        private bool needReload;

        [Header("UI 설정")]
        [SerializeField] private GameObject uiUnderBar;
        
        
        public CharacterController characterController { get; private set; }
        public Animator animator { get; private set; }

        public PlayerStateMachine stateMachine { get; private set; }
        public Dictionary<Type, PlayerStateBase> _states;

        public Action OnPlayerInit;
        public Action OnPlayerDead;
        public Action OnGameClear;
        //TODO : GameClear 추후 설정

        private bool PlayerReady;
        private bool PlayerStatReady;
        
        private Vector3 _aimVector = new Vector3(0.2f,1.8f,0.6f); 
        
        
        

        private void Awake()
        {
            if(!stat.isStatReady) stat.OnStatReady += StatReady;
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            _states = new Dictionary<Type, PlayerStateBase>();
            
            stateMachine = new PlayerStateMachine();
            _states.TryAdd(typeof(PlayerIdleState), new PlayerIdleState(this, stateMachine));
            _states.TryAdd(typeof(PlayerMoveState), new PlayerMoveState(this, stateMachine));
            _states.TryAdd(typeof(PlayerReloadState), new PlayerReloadState(this, stateMachine));
            _states.TryAdd(typeof(PlayerDeadState), new PlayerDeadState(this, stateMachine));
            _states.TryAdd(typeof(PlayerDashState), new PlayerDashState(this, stateMachine));
            _states.TryAdd(typeof(PlayerAimState), new PlayerAimState(this, stateMachine));

            PlayerReady = true;
            PlayerCheck();

        }

        public void PlayerCheck()
        {
            if (PlayerReady && PlayerStatReady)
            {
                OnPlayerInit?.Invoke();
                uiUnderBar.SetActive(true);
            }
            else return;
        }

        public void StatReady()
        {
            PlayerStatReady = true;
            PlayerCheck();
        }
        


        private void Start()
        {
            stateMachine.Initialize(GetState<PlayerIdleState>());
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            ApplyGravity();
            CheckAnyState();
            stateMachine.CurrentState.Update();
        }
        
        public void LateUpdate()
        {
            gunObject.transform.rotation = transform.rotation;
        }

        private void ApplyGravity()
        {
            if (characterController.isGrounded && verticalVelocity < 0)
            {
                verticalVelocity = -2f;
            }
            else
            {
                verticalVelocity += gravity * Time.deltaTime;
            }
        }

        public void CheckAnyState()
        {
            if (stateMachine.CurrentState is PlayerDeadState or PlayerReloadState or PlayerDashState
                ) return;

            if (Input.GetMouseButtonDown(1))
            {
                stateMachine.ChangeState(GetState<PlayerAimState>());
                return;
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                stateMachine.ChangeState(GetState<PlayerReloadState>());
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                stateMachine.ChangeState(GetState<PlayerDashState>());
            }
        }

        public void SetAimCamera(bool ON)
        {
            mainCamera.Priority = ON ? 10 : 20;
            aimCamera.Priority = ON ? 20 : 10;
            
        }

        public void TakeDamage(int amount)
        {
            var isDead = stat.ApplyDamage(amount);
            
            if (isDead)
            {
                stateMachine.ChangeState(GetState<PlayerDeadState>());
            }
        }
        

        public void Shoot()
        {
            if (needReload)
            {
                stateMachine.ChangeState(GetState<PlayerReloadState>());
                return;
            }
            GameObject obj = ObjectPoolManager.Instance.SpawnObject(
                "Bullet", gunObject.transform.position, aimCamera.transform.rotation);
            obj.transform.SetParent(SpawnedBullet);
            stat.ReduceMag();
            
            if (stat.curMagSize <= 0) needReload = true;
        }

        public void Reload()
        {
            stat.Reload();
            needReload = false;
        }
        

        public Vector3 GetVerticalVector()
        {
            return new Vector3(0, verticalVelocity, 0);
        }

        public T GetState<T>() where T : PlayerStateBase
        {
            return (T)_states[typeof(T)];
        }

        public float GetStatus(PlayerStatusType type)
        {
            return stat.GetTypeValue(type);
        }
        
        public void ApplyInvincible()
        {
            stat.ApplyInvincible();
        }

        public void RemoveInvincible()
        {
            stat.RemoveInvincible();
        }
        
        public void ApplyInvincible(float duration)
        {
            stat.ApplyInvincible(duration);
        }

        public void SubscribeEvent(PlayerStatusType type, Action<int> EventHandler)
        {
            stat.SubscribeEvent(type, EventHandler);
        }

        public void UnsubscribeEvent(PlayerStatusType type, Action<int> EventHandler)
        {
            stat.UnsubscribeEvent(type, EventHandler);
        }

    }

    public enum PlayerStatusType
    {
        reloadDuration, dashDuration, curMagSize, maxHealth, currentHealth, 
        moveSpeed, turnSpeed, dashSpeed, aimMoveSpeed, aimTurnSpeed, isInvincible, maxMagSize
    }
}