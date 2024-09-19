using Cutscenes;
using Unity.VisualScripting;
using UnityEngine;
using DialogueSystem.ScriptableObjects;

public class DeathDialogue : DialogueAreaQuestStep
{
    private GameObject _deathRealmToTeleport;
    private GameObject _shoreToTeleport;
    private GameObject _playerToTeleport;
    protected override void OnEnable()
    {
        base.OnEnable();
        CutsceneEvents.OnCutsceneFinished += TeleportPlayer;
        _deathRealmToTeleport = GameObject.Find("DeathArea");
        _shoreToTeleport = GameObject.Find("ShoreArea");
        _playerToTeleport = GameObject.Find("Player");
    }
    protected override void OnDisable()
    {
        base.OnEnable();
        CutsceneEvents.OnCutsceneFinished -= TeleportPlayer;
    }
    
    private void TeleportPlayer()
    {
        _playerToTeleport.GameObject().transform.position = _deathRealmToTeleport.transform.position;
    }
    protected override void AreaEntered(string areaName, string objectName)
    {
        if (areaName.Equals(_deathRealmToTeleport.name))
        {
            AreaEvent.EnterCorrectArea(_deathRealmToTeleport.name);
        }
    }
    protected override void FinishStep(DialogueSO dialogue)
    {
        if (dialoguesInfo.name == dialogue.name)
        {
            Finish();
            Destroy(_deathRealmToTeleport);
            _playerToTeleport.GameObject().transform.position = _shoreToTeleport.transform.position;
            Delete();
        } 
    }
}
