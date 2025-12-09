using Player;
using UnityEngine;

public abstract class PlayerStateBase : MonoBehaviour
{
    protected PlayerController player;
    
    public abstract void Enter();
    
    public abstract void Update();

    public abstract void Exit();
    
}
