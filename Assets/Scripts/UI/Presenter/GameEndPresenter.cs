using Manager;
using Player;

namespace UI.Presenter
{
    public class GameEndPresenter
    {
        private readonly GameEndView _view;
        private readonly PlayerController _player;
        
        public GameEndPresenter(GameEndView view)
        {
            _view = view;
            _player = GlobalStateManager.Instance.player;
            Initialize();
        }

        private void Initialize()
        {
            _player.OnPlayerDead += _view.ShowGameOverPanel;
            _player.OnGameClear += _view.ShowGameClearPanel;
        }
    }
}