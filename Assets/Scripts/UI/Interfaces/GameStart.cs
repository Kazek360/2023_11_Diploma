using Location;
using Other;
using SaveSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Interfaces
{
    public class GameStart : UserInterface
    {
        public override UserInterfaceState State => UserInterfaceState.GameStart;
        
        [SerializeField] private FadeInOutScreen _fadeInOutScreen;
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
            _fadeInOutScreen.StartFade(() => SceneLoader.InvokeOnSceneChanged(startScene));
        }
        
        public void OnContinueClicked()
        {
            DataPersistenceManager.Instance.CurrentLoadMode = LoadMode.FromSaveFile;
            SceneLoader.InvokeOnSceneChanged(DataPersistenceManager.Instance.CurrentGameData.scene.name);
        }
    }
}