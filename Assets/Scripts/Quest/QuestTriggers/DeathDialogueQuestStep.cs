using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeathDialogueQuestStep : QuestStep
{
    [SerializeField] private List<DialogueSO> dialoguesInfo;
    [SerializeField] private QuestSO questToStart;
    
    private DialogueSO _requiredDialogue;
    private GameObject _deathRealmToTeleport;
    private GameObject _shoreToTeleport;
    private GameObject _playerToTeleport;
    private void OnEnable()
    {
        DialogueEvents.OnDialogueFinished += StartFinishingStep;
        CutsceneEvents.OnCutsceneClose += TeleportPlayer;
        
        _deathRealmToTeleport = GameObject.Find("DeathArea");
        _shoreToTeleport = GameObject.Find("ShoreArea");
        _playerToTeleport = GameObject.Find("Wit");
        
        _requiredDialogue = ChooseRequiredDialogue();
        DialogueEvents.UnlockDialogue(_requiredDialogue.id);
    }
    private void OnDisable()
    {
        DialogueEvents.OnDialogueFinished -= StartFinishingStep;
        CutsceneEvents.OnCutsceneClose -= TeleportPlayer;
    }
    
    private DialogueSO ChooseRequiredDialogue()
    {
        foreach (var dialogue in dialoguesInfo)
        {
            bool allFlagsTriggered = true;
            foreach (var dialogueFlag in dialogue.requiredFlags)
            {
                if (!DialogueMarkerManager.GetMarkerById(dialogueFlag.id)._isTriggered)
                { 
                    allFlagsTriggered = false;
                    break;
                }
            }
            if (allFlagsTriggered)
            {
                return dialogue;
            }
        }
        return dialoguesInfo[0];
    }
    private void TeleportPlayer()
    {
        _playerToTeleport.GameObject().transform.position = _deathRealmToTeleport.transform.position;
    }
    
    
    private void StartFinishingStep(DialogueSO dialogue)
    {
        StartCoroutine(FinishStep(dialogue));
    }
    
    private IEnumerator FinishStep(DialogueSO dialogue)
    {
        if (_requiredDialogue.id == dialogue.id)
        {
            //QuestEvents.ProgressQuest(dialogue.relatedQuest.id);
            _playerToTeleport.GameObject().transform.position = _shoreToTeleport.transform.position;
            Destroy(_deathRealmToTeleport);
            yield return new WaitForSeconds(2f);
            QuestEvents.StartQuest(questToStart.id);
            Destroy(gameObject);
        } 
    }
}
