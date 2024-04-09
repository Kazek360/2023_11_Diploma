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
        DialogueEvents.OnDialogueFinished -= StartFinishingStep;
        CutsceneEvents.OnCutsceneClose -= TeleportPlayer;
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
