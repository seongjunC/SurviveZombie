using UnityEngine;

namespace Player
{
    public class PlayerMoveState : PlayerStateBase
    {
        private static readonly int IsMove = Animator.StringToHash("isMove");

        public PlayerMoveState(PlayerController player, PlayerStateMachine stateMachine) :
            base(player, stateMachine)
        {
        }
        
        public override void Enter()
        {
            player.animator.SetBool(IsMove, true);
        }

        public override void Update()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(x, 0, z).normalized;

            if (direction.magnitude < 0.1f)
            {
                stateMachine.ChangeState(player.GetState<PlayerIdleState>());
                return;
            }
            var moveDirection = player.GetCameraDirection(x, z);
            
            Vector3 velocity = (moveDirection * player.GetStatus(PlayerStatusType.moveSpeed)) + player.GetVerticalVector();
            
            player.characterController.Move(velocity * Time.deltaTime);

            if (moveDirection.magnitude >= 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                player.transform.rotation = Quaternion.Slerp(
                    player.transform.rotation, 
                    targetRotation, 
                    Time.deltaTime * player.GetStatus(PlayerStatusType.turnSpeed));
            }
            
        }

        public override void Exit()
        {
            player.animator.SetBool(IsMove, false);
        }
    }
}