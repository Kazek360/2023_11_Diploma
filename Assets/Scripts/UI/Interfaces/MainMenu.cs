using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Interfaces
{
    public class MainMenu : UserInterface
    {
        public override UserInterfaceState State => UserInterfaceState.MainMenu;
        
        public void QuitGame()
        {
            // Zamyka aplikację
            Application.Quit();
    
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
        
        public void onNestorClicked()
        {
            SceneManager.LoadSceneAsync("BlueMainMenu");
        }
        
        public void onWitClicked()
        {
            SceneManager.LoadSceneAsync("MainMenu");
        }
    }
}
