using UnityEngine;

namespace Player
{
    public class PlayerReloadState : PlayerStateBase
    {
        private static readonly int Reload = Animator.StringToHash("Reload");
        private float reloadTimer;
        public PlayerReloadState(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine)
        {
        }

        public override void Enter()
        {
            player.animator.SetTrigger(Reload);
            reloadTimer = player.reloadDuration;
        }

        public override void Update()
        {
            reloadTimer -= Time.deltaTime;

            if (reloadTimer <= 0)
            {
                stateMachine.ChangeState(player.GetState<PlayerIdleState>());
            }
        }

        public override void Exit()
        {
            player.curMagSize = player.maxMagSize;
        }
    }
}