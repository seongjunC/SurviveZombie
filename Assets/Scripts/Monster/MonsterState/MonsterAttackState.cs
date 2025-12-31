using UnityEngine;

namespace Monster.MonsterState
{
    public class MonsterAttackState :MonsterStateBase
    {
        private static readonly int IsMove = Animator.StringToHash("IsMoving");
        private static readonly int attack = Animator.StringToHash("attack");
        
        private float attackCool = 3f;
        private float lastAttack;
        
        public MonsterAttackState(MonsterStateMachine stateMachine, MonsterController controller) : 
            base(stateMachine, controller)
        {
        }

        public override void Enter()
        {
            controller.navMeshAgent.isStopped = true;
            controller.navMeshAgent.SetDestination(controller.transform.position);
            controller.animator.SetBool(IsMove, false);
        }

        public override void Update()
        {
            if (controller.target is null) return;
            
            AnimatorStateInfo stateInfo = controller.animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("ZombieAttack") && stateInfo.normalizedTime < 0.5f) return;
            
            Vector3 direction = ( controller.target.position - controller.transform.position ).normalized;
            direction.y = 0;

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            controller.transform.rotation = Quaternion.Slerp
                (controller.transform.rotation, 
                    lookRotation, 
                    Time.deltaTime * 10f);
            
            float distance = Vector3.Distance(controller.transform.position, controller.target.position);

            if (distance > controller.attackRange)
            {
                stateMachine.ChangeState(controller.GetState<MonsterChaseState>());
                return;
            }

            if (Time.time - lastAttack > attackCool)
            {
                Attack();
            }
        }

        private void Attack()
        {
            lastAttack = Time.time;
            controller.animator.SetTrigger(attack);
        }

        public override void Exit()
        {
        }
    }
}