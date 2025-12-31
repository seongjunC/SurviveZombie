using Manager;
using UnityEngine;

namespace Object
{
    public class Potion : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            var player = GlobalStateManager.Instance.player;
            
            player.OnGameClear?.Invoke();
            Time.timeScale = 0f;
        }
    }
}