using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

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
        
        requiredDialogue = ChooseRequiredDialogue();
        DialogueEvents.UnlockDialogue(requiredDialogue.id);
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
        if (requiredDialogue.id == dialogue.id)
        {
            Destroy(_deathRealmToTeleport);
            yield return new WaitForSeconds(2f);
            QuestEvents.StartQuest(questToStart.id);
            _playerToTeleport.GameObject().transform.position = _shoreToTeleport.transform.position;
            Destroy(gameObject);
        } 
    }
}
