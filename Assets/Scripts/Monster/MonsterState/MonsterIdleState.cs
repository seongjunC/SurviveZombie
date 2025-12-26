using UnityEngine;

namespace Monster.MonsterState
{
    public class MonsterIdleState : MonsterStateBase
    {
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");

        public MonsterIdleState(MonsterStateMachine stateMachine, MonsterController controller) : 
            base(stateMachine, controller)
        {
        }

        public override void Enter()
        {
            //controller.animator.SetBool(IsMoving, false);
            controller.navMeshAgent.isStopped = true;
        }

        public override void Update()
        {
            if (controller.target is null) return;
            
            float distance = Vector3.Distance(controller.transform.position, controller.target.position);

            if (distance <= controller.detectRange)
            {
                stateMachine.ChangeState(controller.GetState<MonsterChaseState>());
            }
        }

        public override void Exit()
        {

        }
    }
}