using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class StartSceneView : MonoBehaviour
    {
        [SerializeField] private GameObject settingPanel;
        [SerializeField] private GameObject Buttons;
        [SerializeField] private Button startButton;
        [SerializeField] private Button settingButton;
        [SerializeField] private Button exitButton;
        
        public UnityAction PressStartButton;
        public UnityAction PressSettingButton;
        public UnityAction PressExitButton;

        public void Init()
        {
            startButton.onClick.AddListener(PressStartButton);
            settingButton.onClick.AddListener(PressSettingButton);
            exitButton.onClick.AddListener(PressExitButton);
            
            OpenPanel(false);
        }

        public void OpenPanel(bool isSetting)
        {
            settingPanel.SetActive(isSetting);
            Buttons.SetActive(!isSetting);
        }
        
    }
}