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
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(x, 0, z).normalized;

            if (direction.magnitude < 0.1f)
            {
                stateMachine.ChangeState(player.GetState<PlayerIdleState>());
                return;
            }
            
            Vector3 moveDirection = (player.transform.forward * z + player.transform.right * x).normalized;
            
            Vector3 velocity = (moveDirection * player.moveSpeed) + player.GetVerticalVector();
            
            player.characterController.Move(velocity * Time.deltaTime);
            
        }

        public override void Exit()
        {
            player.animator.SetBool(IsMove, false);
        }
    }
}