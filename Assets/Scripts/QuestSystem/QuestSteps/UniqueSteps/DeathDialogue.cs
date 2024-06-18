using System.Collections;
using Cutscenes;
using Unity.VisualScripting;
using UnityEngine;
using DialogueSystem.Events;
using DialogueSystem.ScriptableObjects;
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
        DialogueEvents.OnDialogueFinished += FinishStep;
        
        _deathRealmToTeleport = GameObject.Find("DeathArea");
        _shoreToTeleport = GameObject.Find("ShoreArea");
        _playerToTeleport = GameObject.Find("Player");
        
        DialogueEvents.UnlockDialogue(dialoguesInfo.name);
    }
    private void OnDisable()
    {
        CutsceneEvents.OnCutsceneFinished -= TeleportPlayer;
        AreaEvent.OnAreaEnter -= AreaEntered;
        DialogueEvents.OnDialogueFinished -= FinishStep;
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
    private void FinishStep(DialogueSO dialogue)
    {
        if (dialoguesInfo.name == dialogue.name)
        {
            Finish();
            Destroy(_deathRealmToTeleport);
            QuestEvents.StartQuest(questToStart.id);
            _playerToTeleport.GameObject().transform.position = _shoreToTeleport.transform.position;
            Delete();
        } 
    }
}
