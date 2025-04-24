using Location;
using UnityEngine;

namespace Quests.QuestSteps.SceneSteps
{
    public class SceneChangeStep : BaseQuestStep
    {
        [SerializeField] private string sceneName;

        private void Start()
        {
            SceneLoader.InvokeOnSceneChanged(sceneName);
            FinishAndDelete();
        }
    }
}