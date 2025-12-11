namespace Monster.MonsterState
{
    public abstract class MonsterStateBase
    {
        protected MonsterStateMachine stateMachine;

        protected MonsterStateBase(MonsterStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }
    
        public abstract void Enter();
    
        public abstract void Update();

        public abstract void Exit();
    }
}