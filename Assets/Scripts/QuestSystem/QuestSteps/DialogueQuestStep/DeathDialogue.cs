using System.Collections;
using Cutscenes;
using Unity.VisualScripting;
using UnityEngine;
using DialogueSystem.Events;
using DialogueSystem.UI;
using UnityEngine.SceneManagement;

public class DeathDialogue : DialogueQuestStep
{
    [SerializeField] private QuestSO questToStart;
    
    private GameObject _deathRealmToTeleport;
    private GameObject _shoreToTeleport;
    private GameObject _playerToTeleport;
    private void OnEnable()
    {
        CutsceneEvents.OnCutsceneFinished += TeleportPlayer;
        AreaEvent.OnAreaEnter += AreaEntered;
        DialogueEvents.OnDialogueFinished += StartFinishingStep;
        
        _deathRealmToTeleport = GameObject.Find("DeathArea");
        _shoreToTeleport = GameObject.Find("ShoreArea");
        _playerToTeleport = GameObject.Find("Wit");
        
        DialogueEvents.UnlockDialogue(dialoguesInfo.name);
    }
    private void OnDisable()
    {
        CutsceneEvents.OnCutsceneFinished -= TeleportPlayer;
        AreaEvent.OnAreaEnter -= AreaEntered;
        DialogueEvents.OnDialogueFinished -= StartFinishingStep;
    }
    
    private void TeleportPlayer()
    {
        _playerToTeleport.GameObject().transform.position = _deathRealmToTeleport.transform.position;
    }
    private void AreaEntered(string areaName, string objectName)
    {
        if (areaName.Equals(_deathRealmToTeleport.name))
        {
            AreaEvent.EnterCorrectArea(_deathRealmToTeleport.name);
        }
    }
    private void StartFinishingStep(DialogueSO dialogue)
    {
        StartCoroutine(FinishStep(dialogue));
    }
    
    private IEnumerator FinishStep(DialogueSO dialogue)
    {
        if (dialoguesInfo.name == dialogue.name)
        {
            if (((Ink.Runtime.StringValue) GameConfig.DialogueUI.GetVariableState("death_choice")).value.Equals("death"))
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                QuestEvents.FinishQuest(dialogue.relatedQuest.id);
                Destroy(_deathRealmToTeleport);
                yield return new WaitForSeconds(2f);
                QuestEvents.StartQuest(questToStart.id);
                _playerToTeleport.GameObject().transform.position = _shoreToTeleport.transform.position;
                Destroy(gameObject); 
            }
        } 
    }
}
