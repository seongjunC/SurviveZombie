using UnityEngine;

namespace Player
{
    public class PlayerDeadState : PlayerStateBase
    {
        private static readonly int Dead = Animator.StringToHash("Dead");

        public PlayerDeadState(PlayerController player, PlayerStateMachine stateMachine) : 
            base(player, stateMachine)
        {
        }

        public override void Enter()
        {
            player.animator.SetTrigger(Dead);
            player.characterController.enabled = false;
            
            //TODO : 게임 오버 실행
        }

        public override void Update()
        {

        }

        public override void Exit()
        {

        }
    }
}