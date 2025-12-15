using UnityEngine;

namespace Monster.MonsterState
{
    public class MonsterReturnState : MonsterStateBase
    {
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");

        public MonsterReturnState(MonsterStateMachine stateMachine, MonsterController controller) : 
            base(stateMachine, controller)
        {
        }

        public override void Enter()
        {
            controller.animator.SetBool(IsMoving, true);
            controller.navMeshAgent.isStopped = false;
            controller.navMeshAgent.SetDestination(controller.transform.position);
        }

        public override void Update()
        {
            if (controller.target is not null)
            {
                float distanceToTarget = Vector3.Distance(
                    controller.transform.position, controller.target.position);

                if (distanceToTarget <= controller.detectRange)
                {
                    stateMachine.ChangeState(controller.GetState<MonsterChaseState>());
                }
            }

            if (controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance 
                && !controller.navMeshAgent.pathPending)
            {
                stateMachine.ChangeState(controller.GetState<MonsterIdleState>());
            }
        }

        public override void Exit()
        {
            
        }
    }
}