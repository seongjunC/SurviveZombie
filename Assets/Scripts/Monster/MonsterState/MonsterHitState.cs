using UnityEngine;

namespace Monster.MonsterState
{
    public class MonsterHitState : MonsterStateBase
    {
        private static readonly int Hit = Animator.StringToHash("Hit");

        public MonsterHitState(MonsterStateMachine stateMachine, MonsterController controller) : base(stateMachine, controller)
        {
        }

        public override void Enter()
        {
            controller.navMeshAgent.isStopped = true;
            controller.navMeshAgent.SetDestination(controller.transform.position);
            controller.animator.SetTrigger(Hit);
        }

        public override void Update()
        {
            AnimatorStateInfo stateInfo = controller.animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("ZombieHit") && stateInfo.normalizedTime >= 1.0f)
            {
                stateMachine.ChangeState(controller.GetState<MonsterChaseState>());
            }
        }

        public override void Exit()
        {
        }
    }
}