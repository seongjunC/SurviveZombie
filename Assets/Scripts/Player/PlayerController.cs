using UnityEngine;

namespace Player
{
    public class PlayerController
    {
        [Header("Movement")] 
        public float moveSpeed = 5f;
        public float turnSpeed = 30f;
        
        public CharacterController controller;
        
        [Header("FSM")]
        public PlayerStateMachine stateMachine;
        public PlayerIdleState idleState;
        public PlayerMoveState moveState;
    }
}