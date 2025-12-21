using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MagView
    {
        public Image[] magImages;
        public TextMeshProUGUI magText;
        public Color blackColor = Color.black;
        public Color whiteColor = Color.white;

        public void SetMag(int mag,int maxMag, int blackAmount)
        {
            if (blackAmount > 0)
            {
                for (int i = 1; i <= blackAmount; i++)
                {
                    magImages[^i].color = blackColor;
                }
            }
            magText.text = $"{mag} / {maxMag}";
        }
    }
}