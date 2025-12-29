using Manager;
using Player;
using Room;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            _view.RestartButtonAction += BackToMainMenu;
        }
        
        private void BackToMainMenu()
        {
            _player.OnPlayerDead -= _view.ShowGameOverPanel;
            _player.OnGameClear -= _view.ShowGameClearPanel;
            _view.RestartButtonAction -= BackToMainMenu;

            Time.timeScale = 1f;

            _view.ClosePanel();
            
            RoomManager.Instance.Initialize();
            
            SceneManager.LoadScene("GameStartScene");
        }
    }
}