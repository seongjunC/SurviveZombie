using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")] 
        public float moveSpeed = 5f;
        public float turnSpeed = 30f;
     
        public CharacterController characterController { get; private set; }
        public Animator animator { get; private set; }

        public PlayerStateMachine stateMachine { get; private set; }
        public Dictionary<Type, PlayerStateBase> _states;
        
        

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            _states = new Dictionary<Type, PlayerStateBase>();

            stateMachine = new PlayerStateMachine();
            _states.TryAdd(typeof(PlayerIdleState), new PlayerIdleState(this, stateMachine));
            _states.TryAdd(typeof(PlayerMoveState), new PlayerMoveState(this, stateMachine));
        }

        private void Start()
        {
            stateMachine.Initialize(GetState<PlayerIdleState>());
        }

        private void Update()
        {
            stateMachine.CurrentState.Update();
        }
        
        public T GetState<T>() where T : PlayerStateBase
        {
            return (T) _states[typeof(T)];
        }
    }
}