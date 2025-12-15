using System;
using System.Collections.Generic;
using DefaultNamespace;
using Monster.MonsterState;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Monster
{
    public class MonsterController : MonoBehaviour, IPooledObject
    {
        [Header("전투 스탯")] 
        public int maxHp = 100;
        public int currentHp;
        public float detectRange = 15f;
        public float attackRange = 2f;
        public float moveSpeed = 4f;
    
        public Transform target;
        public NavMeshAgent navMeshAgent;
        public Animator animator;
        public Vector3 startPos;
        
        public MonsterStateMachine stateMachine { get; private set; }
        public Dictionary<Type, MonsterStateBase> states;

        public Action OnDeath;

        public void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            startPos = transform.position;
            currentHp = maxHp;
            
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj is not null) target = playerObj.transform;

            stateMachine = new MonsterStateMachine();
            states = new Dictionary<Type, MonsterStateBase>();

            states.TryAdd(typeof(MonsterAttackState), new MonsterAttackState(stateMachine, this));
            states.TryAdd(typeof(MonsterChaseState), new MonsterChaseState(stateMachine, this));
            states.TryAdd(typeof(MonsterDeadState), new MonsterChaseState(stateMachine, this));
            states.TryAdd(typeof(MonsterIdleState), new MonsterIdleState(stateMachine, this));
            states.TryAdd(typeof(MonsterReturnState), new MonsterReturnState(stateMachine, this));
        }
        
        public void OnObjectSpawn()
        {
            currentHp = maxHp;
            
            stateMachine.Initialize(GetState<MonsterIdleState>());
            
            navMeshAgent.enabled = true;
            navMeshAgent.ResetPath();
            
            OnDeath = null;
        }

        public void Update()
        {
            if(currentHp > 0)
                stateMachine.CurrentState.Update();
        }

        public T GetState<T>() where T : MonsterStateBase
        {
            return (T)states[typeof(T)];
        }

        public void TakeDamage(int Damage)
        {
            if (currentHp <= 0) return;
            
            currentHp -= Damage;

            if (currentHp <= 0)
            {
                stateMachine.ChangeState(GetState<MonsterDeadState>());
                OnDeath?.Invoke();
                
                Invoke(nameof(Deactivate), 5f);
            }
        }

        private void Deactivate()
        {
            gameObject.SetActive(false);
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectRange);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }


        
    }
}