using GensouLib.Unity.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleMenu : MonoBehaviour
{
    public string CurrentScene { get; private set; } = "Title";

    // Note add references to the following fields in Unity editor

    public Button NewGameButton;

    public Button LoadButton;

    public Button ConfigButton;

    public Button ExitButton;

    public string ConfigScene = "Config";

    public string SaveLoadScene = "SaveLoad";

    public string MainScene = "Main";

    private void Start()
    {
        SaveLoadGame.LoadConfig();
        VisualNoveCore.RecoverGlobalVariables();
        if (!ValidateButtonReferences())
        {
            Debug.LogError("Some buttons are missing, please add them in Unity editor");
            return;
        }

        BindButtonListeners();
    
    }
    private bool ValidateButtonReferences()
    {
        return NewGameButton != null &&
               LoadButton != null && ConfigButton != null && ExitButton != null;
    }
    private void BindButtonListeners()
    {
        NewGameButton.onClick.AddListener(OnNewGameButtonClick);
        LoadButton.onClick.AddListener(OnLoadButtonClick);
        ConfigButton.onClick.AddListener(OnConfigButtonClick);
        ExitButton.onClick.AddListener(OnExitButtonClick);
    }
    private void OnNewGameButtonClick()
    {
        SceneManager.LoadScene(MainScene);
    }

    private void OnLoadButtonClick()
    {
        SceneManager.LoadScene(SaveLoadScene, LoadSceneMode.Additive);
    }

    private void OnConfigButtonClick()
    {
        SceneManager.LoadScene(ConfigScene, LoadSceneMode.Additive);
    }

    private void OnExitButtonClick()
    {
        Application.Quit();
    }

}
