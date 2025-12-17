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
        [Header("이동 설정")] 
        public float moveSpeed = 5f;
        public float turnSpeed = 30f;
        public float CameraTurnSpeed = 10f;
        public float aimMoveSpeed = 2.5f;
        public float dashSpeed = 15.0f;
        
        [Header("전투 설정")]
        public int currentHealth;
        public int maxHealth = 100;
        public float reloadDuration = 2.0f;
        public int maxMagSize = 30;
        public int curMagSize;

        public bool isInvincible = false;
        public float dashDuration = 0.5f;
        
        [Header("카메라 설정")]
        public CinemachineCamera mainCamera;
        public CinemachineCamera aimCamera;
        private Camera mainCameraComponent;
        
        public GameObject gunObject;
        public Transform SpawnedBullet;
        
        private float gravity = -9.81f;
        private float verticalVelocity;
        
        
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

            currentHealth = maxHealth;
            curMagSize = maxMagSize;
            
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
        }

        private void Update()
        {
            ApplyGravity();
            CheckAnyState();
            RotateCamera();
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
        
        public void RotateCamera()
        {
            if (mainCamera is null) return;
            
            var ray = mainCameraComponent.ScreenPointToRay(Input.mousePosition);
            var groundPlane = new Plane(Vector3.up, transform.position);

            if (!groundPlane.Raycast(ray, out float enter)) return;
            
            Vector3 point = ray.GetPoint(enter);
            Vector3 direction = point - transform.position;
            direction.y = 0;

            if (!(direction.magnitude > 0.01f)) return;
            var targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                targetRotation, 
                Time.deltaTime * CameraTurnSpeed);

        } 

        public void TakeDamage(int amount)
        {
            if (currentHealth <= 0 || isInvincible) return;
            
            currentHealth -= amount;

            if (currentHealth <= 0)
            {
                stateMachine.ChangeState(GetState<PlayerDeadState>());
            }
        }
        

        public void Shoot()
        {
            if (curMagSize <= 0) return;
            GameObject obj = ObjectPoolManager.Instance.SpawnObject(
                "Bullet", gunObject.transform.position, transform.rotation);
            obj.transform.SetParent(SpawnedBullet);
            curMagSize--;
        }
        

        public Vector3 GetVerticalVector()
        {
            return new Vector3(0, verticalVelocity, 0);
        }

        public T GetState<T>() where T : PlayerStateBase
        {
            return (T)_states[typeof(T)];
        }
    }
}