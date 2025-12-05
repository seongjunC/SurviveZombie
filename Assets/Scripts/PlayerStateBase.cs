using UnityEngine;

public abstract class PlayerStateBase : MonoBehaviour
{
    public abstract void Enter();
    
    public abstract void Update();

    public abstract void Exit();
    
}
