using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using DialogueSystem.Events;

public class DeathDialogueQuestStep : DialogueQuestStep
{
    [SerializeField] private QuestSO questToStart;
    
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
        
        DialogueEvents.UnlockDialogue(dialoguesInfo.name);
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
        if (dialoguesInfo.name == dialogue.name)
        {
            Destroy(_deathRealmToTeleport);
            yield return new WaitForSeconds(2f);
            QuestEvents.StartQuest(questToStart.id);
            _playerToTeleport.GameObject().transform.position = _shoreToTeleport.transform.position;
            Destroy(gameObject);
        } 
    }
}
