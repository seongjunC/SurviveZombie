using System;
using UI.Presenter;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class GameEndView : MonoBehaviour
    {
        [SerializeField] private GameObject GameOverPanel;
        [SerializeField] private GameObject GameClearPanel;
        [SerializeField] private Image GameEndPanel;
        [SerializeField] private Button ExitButton;
        //[SerializeField] private Button RestartButton;
        
        // TODO : 재시작 버튼 추가
        
        private Color clearColor = new Color(0,1,1,0.4f);
        private Color gameOverColor = new Color(1,0,0,0.4f);


        private GameEndPresenter _presenter;
        
        public UnityAction RestartButtonAction;
        
        private void Awake()
        {
            _presenter = new GameEndPresenter(this);
            
            //RestartButton.onClick.AddListener(RestartButtonAction);
            
            ExitButton.onClick.AddListener(ExitButtonAction);
            
            ClosePanel();
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

        public void ClosePanel()
        {
            GameEndPanel.gameObject.SetActive(false);
        }

        public void ExitButtonAction()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}