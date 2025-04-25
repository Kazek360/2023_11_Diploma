using Tutorials;
using UnityEngine;

namespace Quests.QuestSteps.OtherSteps
{
    public class TutorialStep : BaseQuestStep
    {
        [SerializeField] private TutorialInfo tutorial;
        
        private void Start()
        {
            TutorialManager.Instance.TriggerTutorial(tutorial);
            FinishAndDelete();
        }
    }
}