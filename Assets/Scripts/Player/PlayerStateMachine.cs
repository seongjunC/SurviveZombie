namespace Player
{
    public class PlayerStateMachine
    {   
        public PlayerStateBase CurrentState { get; private set;}

        public void Initialize(PlayerStateBase startState)
        {
            CurrentState = startState;
            CurrentState.Enter();
        }

        public void ChangeState(PlayerStateBase newState)
        {
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}