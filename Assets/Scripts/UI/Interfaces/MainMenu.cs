using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Interfaces
{
    public class MainMenu : UserInterface
    {
        public override GameState State => GameState.MainMenu;
        
        [SerializeField] private Button continueButton;

        protected new void Awake()
        {
            base.Awake();
        }
        
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
            SceneManager.LoadSceneAsync("Las");
        }
        
        public void OnContinueClicked()
        {
            DataPersistenceManager.Instance.CurrentLoadMode = LoadMode.FromSaveFile;
            SceneManager.LoadSceneAsync(DataPersistenceManager.Instance.CurrentGameData.currentScene);
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
