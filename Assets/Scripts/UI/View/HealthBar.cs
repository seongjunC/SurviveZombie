using Manager;
using Monster;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private MonsterController monster;

        private HPPresenter _presenter;
        [SerializeField] private PlayerController player;

        private void OnEnable()
        {
            if (monster is not null) Initialize(); 
            else if (player is not null) Initialize();

            else
            {
                player = GlobalStateManager.Instance.player;
                player.OnPlayerInit += Initialize;
            }
        }
        
        private void Initialize(){
            if (player is not null)
            {
                Debug.Log("Player Init-> hp");
                _presenter = new HPPresenter(this, player);
                player.OnPlayerInit -= Initialize;
            }
            else if (monster is not null)
            {
                _presenter = new HPPresenter(this, monster);
            }
            else return;
            
            _presenter.Initialize();
        }

        private void OnDisable()
        {
            _presenter.Dispose();
        }
        
        public void SetHealth(int health, int maxHealth)
        {
            slider.maxValue = maxHealth;
            slider.value = health;
            text.text = $"{health} / {maxHealth}";
        }
    }
}