namespace Monster.MonsterState
{
    public abstract class MonsterStateBase
    {
        protected MonsterStateMachine stateMachine;
        protected MonsterController controller;

        protected MonsterStateBase(MonsterStateMachine stateMachine, MonsterController controller)
        {
            this.stateMachine = stateMachine;
            this.controller = controller;
        }
    
        public abstract void Enter();
    
        public abstract void Update();

        public abstract void Exit();
    }
}