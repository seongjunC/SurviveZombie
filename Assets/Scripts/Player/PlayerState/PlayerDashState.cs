using UnityEngine;

namespace Player
{
    public class PlayerDashState : PlayerStateBase
    {
        private static readonly int Dash = Animator.StringToHash("Dash");
        private float dashTimer;
        private float dashSpeed = 15.0f;
        private Vector3 dashDirection;
        
        public PlayerDashState(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine)
        {
        }

        public override void Enter()
        {
            player.animator.SetTrigger(Dash);
            dashTimer = player.dashDuration;
            
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            
            dashDirection = new Vector3(x, 0, z).normalized;

            if (dashDirection.magnitude < 0.1f)
            {
                dashDirection = player.transform.forward;
            }
            
            // TODO : 대시중 무적 상태 진입 
        }

        public override void Update()
        {
            player.characterController.Move(dashSpeed * Time.deltaTime * dashDirection);
            
            dashTimer -= Time.deltaTime;
            
            if (dashTimer <= 0)
            {
                stateMachine.ChangeState(player.GetState<PlayerIdleState>());
            }
        }

        public override void Exit()
        {
            //TODO: 무적 해제
        }
    }
}