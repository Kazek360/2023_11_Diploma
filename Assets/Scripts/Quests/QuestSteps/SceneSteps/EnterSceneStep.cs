using Location;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Quests.QuestSteps.SceneSteps
{
    public class EnterSceneStep : BaseQuestStep
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
        
        private void FinishStep(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (SceneInfo.Instance == null)
            {
                return;
            }
            if (sceneName.Equals(SceneInfo.Instance.currentScene))
            {
                FinishAndDelete();
            }
        }
    }
}