using UnityEngine;
using DialogueSystem.Events;

public class DialogueWithTeleportQuestStep : DialogueQuestStep
{
    private GameObject _npcToTeleport;
    private GameObject _placeToTeleport;
    
    private void OnEnable()
    {
        DialogueEvents.OnDialogueFinished += FinishStep;

        _npcToTeleport = GameObject.Find("Mother");
        _placeToTeleport = GameObject.Find("TeleportPlace");
        
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
            _npcToTeleport.transform.position = _placeToTeleport.transform.position;
            Finish();
        } 
    }
}
