using System.Collections;
using System.Collections.Generic;
using Dialogues;
using Other;
using Tutorials;
using UI;
using UnityEngine;

public class ChapterIntroductionStep: BaseQuestStep
{
    [SerializeField] private IntroductionInfo introduction;
    // [SerializeField] private TutorialInfo inventoryTutorial;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        ChapterIntroduction.OnIntroductionFinish += FinishStep;
    }

    protected override void OnDestroy()
    {
        base.OnEnable();
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
        UserInterfaceManager.Instance.ChangeState(UserInterfaceState.Exploration);
        //TutorialManager.Instance.TriggerTutorial(inventoryTutorial);
        FinishAndDelete();
    }
}
