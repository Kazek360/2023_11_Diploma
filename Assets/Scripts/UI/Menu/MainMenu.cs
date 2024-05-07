using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button continueButton; 
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
    public void OnContinueClicked()
    {
        SceneManager.LoadSceneAsync("Las");
    }
    public void OnSettingsClicked()
    {
        Debug.Log("Settings");
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