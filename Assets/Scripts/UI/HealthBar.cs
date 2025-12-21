using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Scrollbar scrollbar;
        [SerializeField] private TextMeshProUGUI text;

        public void SetHealth(int health)
        {
            scrollbar.value = health;
            text.text = health.ToString();
        }
    }
}