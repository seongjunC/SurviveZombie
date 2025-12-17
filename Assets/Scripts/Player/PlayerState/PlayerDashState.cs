using UnityEngine;

namespace Player
{
    public class PlayerDashState : PlayerStateBase
    {
        private static readonly int Dash = Animator.StringToHash("Dash");
        private float dashTimer;
        private Vector3 dashDirection;
        
        public PlayerDashState(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine)
        {
        }

        public override void Enter()
        {
            player.animator.SetTrigger(Dash);
            dashTimer = player.dashDuration;

            player.isInvincible = true;
            
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            
            dashDirection = new Vector3(x, 0, z).normalized;

            if (dashDirection.magnitude < 0.1f)
            {
                dashDirection = player.transform.forward;
            }
            else
            {
                player.transform.rotation = Quaternion.LookRotation(dashDirection);
            }

        }

        public override void Update()
        {
            Vector3 moveVelocity = (dashDirection * player.dashSpeed) + player.GetVerticalVector();
            
            player.characterController.Move(moveVelocity * Time.deltaTime);
            
            dashTimer -= Time.deltaTime;
            
            if (dashTimer <= 0)
            {
                stateMachine.ChangeState(player.GetState<PlayerIdleState>());
            }
        }

        public override void Exit()
        {
            player.isInvincible = false;
        }
    }
}