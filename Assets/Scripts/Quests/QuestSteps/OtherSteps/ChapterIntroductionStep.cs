using System.Collections;
using Other;
using Tutorials;
using UI;
using UnityEngine;

namespace Quests.QuestSteps.OtherSteps
{
    public class ChapterIntroductionStep: BaseQuestStep
    {
        [SerializeField] private IntroductionInfo introduction;
    
        protected override void OnEnable()
        {
            base.OnEnable();
            ChapterIntroduction.OnIntroductionFinish += FinishStep;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            ChapterIntroduction.OnIntroductionFinish -= FinishStep;
        }
    
        private IEnumerator Start()
        {
            while (!UserInterfaceManager.isInitialized)
            {
                yield return null;
            }
            UserInterfaceManager.Instance.ChangeState(UserInterfaceState.ChapterIntroduction);
            ChapterIntroduction.InvokeOnIntroductionStart(introduction);
        }
    
        private void FinishStep()
        {
            FinishAndDelete();
        }
    }
}