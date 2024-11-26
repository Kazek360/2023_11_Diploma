using Location;
using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Interfaces
{
    public class MainMenu : UserInterface
    {
        public override UserInterfaceState State => UserInterfaceState.MainMenu;
        
        [SerializeField] private Button continueButton;
        [SerializeField] private string startScene;
        
        private void Start()
        {
            if (!DataPersistenceManager.Instance.HasGameData())
            {
                continueButton.interactable = false;
            }
        }
    
        public void OnNewGameClicked()
        {
            GameConfig.abilityLibrary.Load();
            PlayerStats.GetInstance().Load();
            DataPersistenceManager.Instance.CurrentLoadMode = LoadMode.NewGame;
            SceneLoader.InvokeOnSceneChanged(startScene);
        }
        
        public void OnContinueClicked()
        {
            DataPersistenceManager.Instance.CurrentLoadMode = LoadMode.FromSaveFile;
            SceneLoader.InvokeOnSceneChanged(DataPersistenceManager.Instance.CurrentGameData.scene.sceneName);
        }
        
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
