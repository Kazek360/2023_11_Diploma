using UnityEngine;

namespace Quests.QuestSteps.OtherSteps
{
    public class SkipStep : BaseQuestStep
    {
        [SerializeField] private string whatStepIsIt;
        
        private void Start()
        {
            FinishAndDelete();
        }
    }
}