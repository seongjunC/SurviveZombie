using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI text;

        public void SetHealth(int health, int maxHealth)
        {
            slider.maxValue = maxHealth;
            slider.value = health;
            text.text = $"{health} / {maxHealth}";
        }
    }
}