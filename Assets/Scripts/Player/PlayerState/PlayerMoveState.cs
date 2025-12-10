using UnityEngine;

namespace Player
{
    public class PlayerMoveState : PlayerStateBase
    {
        private static readonly int IsMove = Animator.StringToHash("isMove");

        public PlayerMoveState(PlayerController player, PlayerStateMachine stateMachine) : 
            base(player, stateMachine) { }
        
        public override void Enter()
        {
            player.animator.SetBool(IsMove, true);
        }

        public override void Update()
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 direction = new Vector3(x, 0, z).normalized;

            if (direction.magnitude < 0.1f)
            {
                stateMachine.ChangeState(player.GetState<PlayerIdleState>());
                return;
            }
            
            
            
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            player.transform.rotation = 
                Quaternion.Slerp(
                    player.transform.rotation, 
                    targetRotation, 
                    Time.deltaTime * player.turnSpeed);

            player.characterController.Move(
                player.moveSpeed * Time.deltaTime * direction);
        }

        public override void Exit()
        {
            player.animator.SetBool(IsMove, false);
        }
    }
}