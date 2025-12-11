using Monster.MonsterState;

namespace Monster
{
    public class MonsterStateMachine
    {
        public MonsterStateBase CurrentState { get; private set;}

        public void Initialize(MonsterStateBase startState)
        {
            CurrentState = startState;
            CurrentState.Enter();
        }

        public void ChangeState(MonsterStateBase newState)
        {
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}