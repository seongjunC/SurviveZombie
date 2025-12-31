using System;
using System.Collections.Generic;
using DefaultNamespace;
using Manager;
using Monster.MonsterState;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Monster
{
    public class MonsterController : MonoBehaviour, IPooledObject
    {
        private static readonly int Hit = Animator.StringToHash("Hit");

        [Header("전투 스탯")] 
        public int maxHp = 100;
        private int _currentHp;
        public int currentHp
        {
            get => _currentHp;
            private set
            {
                _currentHp = value;
                
                if (currentHp < 0) currentHp = 0;
                
                OnHealthChanged?.Invoke(currentHp);
            }
        }
        public float detectRange = 5f;
        public float attackRange = 1f;
        public float moveSpeed = 4f;
        public int attackDamage = 25;
        [SerializeField] private float despawnTime = 2f;
    
        public Transform target;
        public Vector3 startPos;
        public NavMeshAgent navMeshAgent;
        public Animator animator;
        
        public MonsterStateMachine stateMachine { get; private set; }
        public Dictionary<Type, MonsterStateBase> states;

        public Action OnDeath;

		public GameObject healthBar;
        
        public Action<int> OnHealthChanged;
        
        public PlayerController player;

        private ZombieAttackController attackCont;

        public void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            attackCont = GetComponent<ZombieAttackController>();
            startPos = transform.position;
            currentHp = maxHp;

            
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj is not null) target = playerObj.transform;
            

            stateMachine = new MonsterStateMachine();
            states = new Dictionary<Type, MonsterStateBase>();

            states.TryAdd(typeof(MonsterAttackState), new MonsterAttackState(stateMachine, this));
            states.TryAdd(typeof(MonsterChaseState), new MonsterChaseState(stateMachine, this));
            states.TryAdd(typeof(MonsterDeadState), new MonsterDeadState(stateMachine, this));
            states.TryAdd(typeof(MonsterIdleState), new MonsterIdleState(stateMachine, this));
            states.TryAdd(typeof(MonsterReturnState), new MonsterReturnState(stateMachine, this));
            states.TryAdd(typeof(MonsterHitState), new MonsterHitState(stateMachine, this));
            
            healthBar.SetActive(true);
        }
        
        public void OnObjectSpawn()
        {
            currentHp = maxHp;
            
            player = GlobalStateManager.Instance.player;
            
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


		public void LateUpdate()
        {
            healthBar.transform.rotation = 
                player.mainCameraComponent.transform.rotation;
            
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
                SoundManager.Instance.PlaySFXAtPoint(SoundType.SFX_ZombieDead, transform.position, 0.5f);
                
                stateMachine.ChangeState(GetState<MonsterDeadState>());
                OnDeath?.Invoke();
                
                Invoke(nameof(Deactivate), despawnTime);
            }
            else
            {
                SoundManager.Instance.PlaySFXAtPoint(SoundType.SFX_ZombieReact, transform.position, 0.5f);
                animator.SetTrigger(Hit);
                stateMachine.ChangeState(GetState<MonsterChaseState>());
            }
        }

        public void InitMonster(int savedHp = 0)
        {
            currentHp = savedHp > 0 ? savedHp : maxHp;
        }

        private void Deactivate()
        {
            healthBar.SetActive(false);
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