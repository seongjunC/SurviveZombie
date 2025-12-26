using UnityEngine;

namespace Monster.MonsterState
{
    public class MonsterDeadState : MonsterStateBase
    {
        private static readonly int Dead = Animator.StringToHash("Dead");

        public MonsterDeadState(MonsterStateMachine stateMachine, MonsterController controller) : 
            base(stateMachine, controller)
        {
        }

        public override void Enter()
        {
            controller.animator.SetTrigger(Dead);
            controller.navMeshAgent.enabled = false;
            
            Collider col = controller.GetComponent<Collider>();
            
            if(col is not null) col.enabled = false;
        }

        public override void Update()
        {

        }

        public override void Exit()
        {

        }
    }
}