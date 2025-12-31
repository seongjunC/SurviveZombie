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
            dashTimer = player.GetStatus(PlayerStatusType.dashDuration);

            player.ApplyInvincible();
            
            dashDirection = player.transform.forward;

        }

        public override void Update()
        {
            Vector3 moveVelocity = (dashDirection * player.GetStatus(PlayerStatusType.dashSpeed)) + player.GetVerticalVector();
            
            player.characterController.Move(moveVelocity * Time.deltaTime);
            
            dashTimer -= Time.deltaTime;
            
            if (dashTimer <= 0)
            {
                stateMachine.ChangeState(player.GetState<PlayerIdleState>());
            }
        }

        public override void Exit()
        {
            player.RemoveInvincible();
        }
    }
}