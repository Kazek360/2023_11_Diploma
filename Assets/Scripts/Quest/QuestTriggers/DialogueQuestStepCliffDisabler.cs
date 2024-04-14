using UnityEngine;
using DialogueSystem.Events;

public class DialogueQuestStepCliffDisabler : DialogueQuestStep
{
    private GameObject _cliffArea;
    private void OnEnable()
    {
        DialogueEvents.OnDialogueFinished += FinishStep;

        _cliffArea = GameObject.Find("CliffArea");
        _cliffArea.SetActive(false);
        
        DialogueEvents.UnlockDialogue(dialoguesInfo.name);
    }
    private void OnDisable()
    {
        DialogueEvents.OnDialogueFinished-= FinishStep;
    }
    
    private void FinishStep(DialogueSO dialogue)
    {
        if (dialoguesInfo.name == dialogue.name)
        {
            _cliffArea.SetActive(true);
            Finish();
        } 
    }
}
