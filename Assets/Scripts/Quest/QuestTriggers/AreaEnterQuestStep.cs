using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEnterQuestStep : QuestStep
{
    private void OnEnable()
    {
        AreaEvent.OnAreaEnter += FinishStep;
    }
    private void OnDisable()
    {
        AreaEvent.OnAreaEnter -= FinishStep;
    }
    private void FinishStep()
    {
        Finish();
    }
}
