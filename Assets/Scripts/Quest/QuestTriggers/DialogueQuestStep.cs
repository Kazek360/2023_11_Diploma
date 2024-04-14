using System.Collections.Generic;
using UnityEngine;
using DialogueSystem.Events;

public class DialogueQuestStep : QuestStep
{
    [SerializeField] protected DialogueSO dialoguesInfo;
    [SerializeField] protected List<string> areasToDisableNames;
    protected List<GameObject> areasToDisable = new List<GameObject>();
    private void OnEnable()
    {
        DialogueEvents.OnDialogueFinished += FinishStep;
        DialogueEvents.UnlockDialogue(dialoguesInfo.name);
        for (var i = 0; i < areasToDisableNames.Count; i++)
        {
            areasToDisable.Add(GameObject.Find(areasToDisableNames[i]));
            areasToDisable[i].SetActive(false);
        }
    }
    private void OnDisable()
    {
        DialogueEvents.OnDialogueFinished-= FinishStep;
    }
    
    private void FinishStep(DialogueSO dialogue)
    {
        if (dialoguesInfo.name == dialogue.name)
        {
            for (var i = 0; i < areasToDisableNames.Count; i++)
            {
                areasToDisable[i].SetActive(true);
            }
            Finish();
        } 
    }
}
