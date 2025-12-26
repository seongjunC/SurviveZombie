using Player;
using UnityEngine;

public abstract class PlayerStateBase
{
    protected PlayerController player;
    protected PlayerStateMachine stateMachine;

    protected PlayerStateBase(PlayerController player, PlayerStateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
    }
    
    public abstract void Enter();
    
    public abstract void Update();

    public abstract void Exit();
    
}
