using UnityEngine;

namespace Monster.MonsterState
{
    public class MonsterChaseState : MonsterStateBase
    {
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");

        public MonsterChaseState(MonsterStateMachine stateMachine, MonsterController controller) : 
            base(stateMachine, controller)
        {
        }

        public override void Enter()
        {
            controller.animator.SetBool(IsMoving, true);
            controller.navMeshAgent.isStopped = false;
        }

        public override void Update()
        {
            if (controller.target is null) return;
            
            float distance = Vector3.Distance(controller.transform.position, controller.target.position);

            if (distance <= controller.attackRange)
            {
                stateMachine.ChangeState(controller.GetState<MonsterAttackState>());
                return;
            }

            if (distance > controller.detectRange * 1.8f)
            {
                stateMachine.ChangeState(controller.GetState<MonsterReturnState>());
                return;
            }
            
            controller.navMeshAgent.SetDestination(controller.target.position);
        }

        public override void Exit()
        {
            
        }
    }
}