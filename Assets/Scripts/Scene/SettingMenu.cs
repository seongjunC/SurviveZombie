using Manager;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Scene
{
    public class SettingMenu : MonoBehaviour
    {
        [Header("사운드 설정")]
        [SerializeField] private GameObject soundPanel;
        [SerializeField] private Slider BGMSlider;
        [SerializeField] private Slider SFXSlider;
        [SerializeField] private Slider VoiceSlider;
        [SerializeField] private Button SoundButton;
        
        [Header("언어 설정")]
        [SerializeField] private GameObject languagePanel;
        [SerializeField] private TMP_Dropdown voiceDropdown;
        [SerializeField] private Button LanguageButton;
        
        [SerializeField] private Button backButton;
        [SerializeField] private Button saveButton;
        [SerializeField] private Button resetButton;

        private enum SettingType
        {
            Sound,
            Language
        }
        
        private SettingType settingType = SettingType.Sound;


        private void Start()
        {
            backButton.onClick.AddListener(BackButton);
            saveButton.onClick.AddListener(SaveButton);
            resetButton.onClick.AddListener(ResetButton);
            SoundButton.onClick.AddListener(OnSoundSetting);
            LanguageButton.onClick.AddListener(OnLanguageSetting);
        }

        private void OnSoundSetting()
        {
            soundPanel.SetActive(true);
            languagePanel.SetActive(false);

            settingType = SettingType.Sound;
        }
        
        private void OnLanguageSetting()
        {
            soundPanel.SetActive(false);
            languagePanel.SetActive(true);
            
            settingType = SettingType.Language;
        }
        
        private void SaveButton()
        {
            if (settingType == SettingType.Sound)
            {
                PlayerPrefs.SetFloat("BGMVolume", BGMSlider.value);
                PlayerPrefs.SetFloat("SFXVolume", SFXSlider.value);
                PlayerPrefs.SetFloat("VoiceVolume", VoiceSlider.value);
                SoundManager.Instance.AudioSetting(BGMSlider.value, SFXSlider.value, VoiceSlider.value);
            }
            else if (settingType is SettingType.Language)
            {
                GlobalStateManager.Instance.voiceType = (VoiceType) voiceDropdown.value;
            }
        }

        private void ResetButton()
        {
            BGMSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);
            SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
            VoiceSlider.value = PlayerPrefs.GetFloat("VoiceVolume", 1f);
            voiceDropdown.value = (int) GlobalStateManager.Instance.voiceType;
        }

        private void BackButton()
        {
            SaveButton();
        }
        
        
        
    }
}