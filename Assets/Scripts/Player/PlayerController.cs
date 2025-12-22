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
        private Camera mainCameraComponent;
        
        public GameObject gunObject;
        public Transform SpawnedBullet;
        
        private float gravity = -9.81f;
        private float verticalVelocity;
        
        private bool needReload;
        
        
        public CharacterController characterController { get; private set; }
        public Animator animator { get; private set; }

        public PlayerStateMachine stateMachine { get; private set; }
        public Dictionary<Type, PlayerStateBase> _states;
        
        

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            mainCameraComponent = Camera.main;
            _states = new Dictionary<Type, PlayerStateBase>();
            
            stateMachine = new PlayerStateMachine();
            _states.TryAdd(typeof(PlayerIdleState), new PlayerIdleState(this, stateMachine));
            _states.TryAdd(typeof(PlayerMoveState), new PlayerMoveState(this, stateMachine));
            _states.TryAdd(typeof(PlayerReloadState), new PlayerReloadState(this, stateMachine));
            _states.TryAdd(typeof(PlayerDeadState), new PlayerDeadState(this, stateMachine));
            _states.TryAdd(typeof(PlayerDashState), new PlayerDashState(this, stateMachine));
            _states.TryAdd(typeof(PlayerAimState), new PlayerAimState(this, stateMachine));
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
            //RotateCamera();
            stateMachine.CurrentState.Update();
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
                return;
            }
        }

        public void SetAimCamera(bool ON)
        {
            mainCamera.Priority = ON ? 10 : 20;
            aimCamera.Priority = ON ? 20 : 10;
        }
        
        // public void RotateCamera()
        // {
        //     if (mainCamera is null) return;
        //     
        //     var ray = mainCameraComponent.ScreenPointToRay(Input.mousePosition);
        //     var groundPlane = new Plane(Vector3.up, transform.position);
        //
        //     if (!groundPlane.Raycast(ray, out float enter)) return;
        //     
        //     Vector3 point = ray.GetPoint(enter);
        //     Vector3 direction = point - transform.position;
        //     direction.y = 0;
        //
        //     if (!(direction.magnitude > 0.01f)) return;
        //
        //     float speedMultiplier = 1.0f;
        //     speedMultiplier = Mathf.Clamp01(direction.magnitude / 10.0f);
        //     
        //     var targetRotation = Quaternion.LookRotation(direction);
        //     transform.rotation = Quaternion.Slerp(
        //         transform.rotation, 
        //         targetRotation, 
        //         Time.deltaTime * CameraTurnSpeed * speedMultiplier);
        //
        // } 

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
                "Bullet", gunObject.transform.position, transform.rotation);
            obj.transform.SetParent(SpawnedBullet);
            stat.ReduceMag();
        }

        public void Reload()
        {
            stat.Reload();
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
        moveSpeed, turnSpeed, dashSpeed, aimMoveSpeed, aimTurnSpeed, isInvincible
    }
}