using Monster;
using Player;
using UnityEngine;

namespace UI.Presenter
{
    public class MagPresenter
    {
        private MagView _view;
        private PlayerController _player;

        private int maxMagSize;

        public MagPresenter(MagView view, PlayerController player)
        {
            _view = view;
            _player = player;
        }

        public void Initialize()
        {
            maxMagSize = (int)_player.GetStatus(PlayerStatusType.maxMagSize);
            _player.SubscribeEvent(PlayerStatusType.curMagSize, UpdateView);
        
            UpdateView((int)_player.GetStatus(PlayerStatusType.curMagSize));
        }
        

        public void UpdateView(int magAmount)
        {
            _view.SetMag(magAmount, maxMagSize);
        }

        public void Dispose()
        {
            _player.UnsubscribeEvent(PlayerStatusType.curMagSize, UpdateView);
        }
    }
}