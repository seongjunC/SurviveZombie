using Manager;
using Player;
using TMPro;
using UI.Presenter;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MagView : MonoBehaviour
    {
        public Image[] magImages;
        public TextMeshProUGUI magText;
        public Color blackColor = Color.black;
        public Color whiteColor = Color.white;
        
        private PlayerController player;
        
        private MagPresenter _presenter;

        public void OnEnable()
        {
            player = GlobalStateManager.Instance.player;
            
            Initialize();
            foreach (var image in magImages) image.color = whiteColor;
        }

        private void Initialize()
        {
            _presenter = new MagPresenter(this, player);

            
            _presenter.Initialize();
        }
        
        private void OnDisable()
        {
            _presenter.Dispose();
        }

        public void SetMag(int mag,int maxMag)
        {
            int len = magImages.Length;
            var slotPer = maxMag / len;
            if ( maxMag % len >0) slotPer++;
            var blackAmount = len - (mag / slotPer);
            if ( mag % slotPer > 0) blackAmount--;
            if (blackAmount > 0)
            {
                for (int i = 1; i <= blackAmount; i++)
                {
                    magImages[^i].color = blackColor;
                }
            }
            else foreach (var image in magImages) image.color = whiteColor;
            
            magText.text = $"{mag} / {maxMag}";
        }
    }
}