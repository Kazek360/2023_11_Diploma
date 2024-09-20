using InputSystem;
using UI;
using UI.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : UserInterface
{
    public override GameState State => GameState.PauseMenu;

    public override void OnEnter()
    {
        base.OnEnter();
        Time.timeScale = 0;
        InputManager.Instance.Controlls.Player.Disable();
        InputManager.Instance.Controlls.UI.Disable();
    }
    public override void OnExit()
    {
        base.OnExit();
        Time.timeScale = 1;
        InputManager.Instance.Controlls.Player.Enable();
        InputManager.Instance.Controlls.UI.Enable();
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
