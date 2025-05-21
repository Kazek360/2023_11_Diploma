using Location;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Quests.QuestSteps.SceneSteps
{
    public class SceneChangeStep : BaseQuestStep
    {
        [SerializeField] private string sceneName;

        protected override void OnEnable()
        {
            base.OnEnable();
            SceneManager.sceneLoaded += FinishStep;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            SceneManager.sceneLoaded -= FinishStep;
        }
        
        private void Start()
        {
            SceneLoader.InvokeOnSceneChanged(sceneName);
        }

        private void FinishStep(Scene scene, LoadSceneMode loadSceneMode)
        {
            FinishAndDelete();
        }
    }
}