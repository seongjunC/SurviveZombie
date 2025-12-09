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
            player.animator.SetBool(Dead, true);
        }

        public override void Update()
        {
            throw new System.NotImplementedException();
        }

        public override void Exit()
        {
            throw new System.NotImplementedException();
        }
    }
}