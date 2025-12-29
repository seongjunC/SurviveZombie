using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneScript : MonoBehaviour
{
    [SerializeField] private StartSceneView startSceneView;

    public void Start()
    {
        startSceneView.PressStartButton += PressStartButton;
        startSceneView.PressSettingButton += PressSettingButton;
        startSceneView.PressExitButton += PressExitButton;
        startSceneView.Init();
    }

    private void PressStartButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void PressSettingButton()
    { 
        startSceneView.OpenPanel(true);
    }

    public void BackToMainMenu()
    {
        startSceneView.OpenPanel(false);
    }
    
    public void SettingMenuOpen()
    {
        //TODO
    }

    private void PressExitButton()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
