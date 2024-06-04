using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    [SerializeField] private SaveSlotMenu saveSlotMenu;
    [SerializeField] private SettingsMenu settingsMenu;
    private void Start()
    {
        if (!DataPersistenceManager.Instance.HasGameData())
        {
            continueButton.interactable = false;
        }
    }

    public void OnNewGameClicked()
    {
        DataPersistenceManager.Instance.NewGame();
        SceneManager.LoadSceneAsync("Las");
    }
    
    public void OnLoadGameClicked()
    {
        gameObject.SetActive(false);
        saveSlotMenu.ActivateMenu("load");
    }
    
    public void OnContinueClicked()
    {
        SceneManager.LoadSceneAsync(DataPersistenceManager.Instance.GameData.currentScene);
    }
    public void OnSettingsClicked()
    {
        gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(true);
    }
    
    public void QuitGame()
    {
        // Zamyka aplikację
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}