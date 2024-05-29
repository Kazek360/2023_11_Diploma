using InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject contentParent;
    [SerializeField] private SaveSlotMenu saveSlotMenu;
    [SerializeField] private GameObject buttons;
    
    private void OnEnable()
    {
        InputEvents.OnPauseToggle += TogglePause;
    }
    private void OnDisable()
    {
        InputEvents.OnPauseToggle -= TogglePause;
    }

    private void Start()
    {
        contentParent.SetActive(false);
    }

    private void TogglePause()
    {
        if (contentParent.activeSelf)
        {
            contentParent.SetActive(false);
            saveSlotMenu.gameObject.SetActive(false);
            buttons.SetActive(true);
            GameConfig.QuestTracker.EnableText();
            Time.timeScale = 1;
        }
        else
        {
            contentParent.SetActive(true);
            GameConfig.QuestTracker.DisableText();
            Time.timeScale = 0;
        }
    }
    
    public void OnSaveGameClicked()
    {
        buttons.SetActive(false);
        saveSlotMenu.ActivateMenu("save");
    }
    
    public void OnLoadGameClicked()
    {
        buttons.SetActive(false);
        saveSlotMenu.ActivateMenu("load");
    }
    
    public void OnToMainMenuClicked()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("MainMenu");
    }
    public void OnQuitGameClicked()
    {
        Application.Quit();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
