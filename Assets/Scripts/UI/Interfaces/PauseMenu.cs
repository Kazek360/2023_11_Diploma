using InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Interfaces
{
    public class PauseMenu : UserInterface
    {
        public override UserInterfaceState State => UserInterfaceState.PauseMenu;

        public override void OnEnter()
        {
            base.OnEnter();
            Time.timeScale = 0;
            InputManager.Instance.Controlls.Player.Disable();
        }
        public override void OnExit()
        {
            base.OnExit();
            Time.timeScale = 1;
            InputManager.Instance.Controlls.Player.Enable();
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
}
