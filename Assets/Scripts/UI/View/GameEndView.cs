using System;
using UI.Presenter;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace UI
{
    public class GameEndView : MonoBehaviour
    {
        [SerializeField] private GameObject GameOverPanel;
        [SerializeField] private GameObject GameClearPanel;
        [SerializeField] private Image GameEndPanel;
        
        private Color clearColor = Color.cyan;
        private Color gameOverColor = new Color(1,0,0,0.4f);


        private GameEndPresenter _presenter;
        
        private void Awake()
        {
            _presenter = new GameEndPresenter(this);
        }

        public void ShowGameOverPanel()
        {
            GameEndPanel.gameObject.SetActive(true);
            GameOverPanel.SetActive(true);
            GameClearPanel.SetActive(false);
            GameEndPanel.color = gameOverColor;
        }

        public void ShowGameClearPanel()
        {
            GameEndPanel.gameObject.SetActive(true);
            GameOverPanel.SetActive(false);
            GameClearPanel.SetActive(true);
            GameEndPanel.color = clearColor;
        }
    }
}