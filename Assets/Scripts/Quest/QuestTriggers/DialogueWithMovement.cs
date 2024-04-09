using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueWithMovement : QuestStep
{
    [SerializeField] private List<DialogueSO> dialoguesInfo;
    private DialogueSO _requiredDialogue;
    //private GameObject _npcToTeleport;
    //private GameObject _placeToTeleport;
    
    private void OnEnable()
    {
        DialogueEvents.OnDialogueFinished += FinishStep;

        //_npcToTeleport = GameObject.Find("Hryc");
        //_placeToTeleport = GameObject.Find("TeleportPoint");
        
        foreach (var dialogue in dialoguesInfo)
        {
            if (dialogue.requiredFlag is not null)
            {
                if (DialogueMarkerManager.GetMarkerById(dialogue.requiredFlag.id)._isTriggered)
                {
                    _requiredDialogue = dialogue;
                    break;
                }
            }
        }
        
        if (_requiredDialogue is null)
        {
            _requiredDialogue = dialoguesInfo[0];
        }
        
        DialogueEvents.UnlockDialogue(_requiredDialogue.id);
    }
    private void OnDisable()
    {
        DialogueEvents.OnDialogueFinished-= FinishStep;
    }

    private void FinishStep(DialogueSO dialogue)
    {
        if (_requiredDialogue.id == dialogue.id)
        {
            PathEvents.StartMovement();
            //_npcToTeleport.GameObject().transform.position = _placeToTeleport.transform.position;
            //Destroy(_placeToTeleport);
            Finish();
        } 
    }
}