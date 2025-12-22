using pattern.Singleton;
using Player;
using UnityEngine;

namespace Manager
{
    public class GlobalStateManager : Singleton<GlobalStateManager>
    {
        [SerializeField] public PlayerController player;
    }
}