using System.Collections;
using Other;
using Tutorials;
using UI;
using UnityEngine;

namespace Quests.QuestSteps.UniqueSteps
{
    public class ChapterIntroductionStep: BaseQuestStep
    {
        [SerializeField] private IntroductionInfo introduction;
        [SerializeField] private TutorialInfo firstTutorial;
    
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
            TutorialManager.Instance.TriggerTutorial(firstTutorial);
            FinishAndDelete();
        }
    }
}