using UnityEngine;

namespace Player
{
    public class PlayerIdleState : PlayerStateBase
    {
        public PlayerIdleState(PlayerController player, PlayerStateMachine stateMachine) : 
            base(player, stateMachine) { }

        public override void Enter()
        {
            // controller.Animator.SetBool("Idle", true);
        }

        public override void Update()
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            if (Mathf.Abs(x) > 0.1f || Mathf.Abs(z) > 0.1f)
            {
                stateMachine.ChangeState(player.GetState<PlayerMoveState>());
            }
        }

        public override void Exit()
        {
           
        }
    }
}