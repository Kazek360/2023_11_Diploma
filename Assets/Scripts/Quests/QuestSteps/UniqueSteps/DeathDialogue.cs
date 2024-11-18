using Cutscenes;
using DialogueSystem;
using UnityEngine;

public class DeathDialogue : DialogueQuestStep
{
    private GameObject _deathRealmToTeleport;
    private GameObject _shoreToTeleport;
    private GameObject _playerToTeleport;
    protected override void OnEnable()
    {
        base.OnEnable();
        AreaEvent.OnAreaEnter += AreaEntered;
        CutsceneEvents.OnCutsceneFinished += TeleportPlayer;
        _deathRealmToTeleport = GameObject.Find("DeathArea");
        _shoreToTeleport = GameObject.Find("ShoreArea");
        _playerToTeleport = GameObject.Find("Player");
    }
    
    private void TeleportPlayer()
    {
        _playerToTeleport.gameObject.transform.position = _deathRealmToTeleport.transform.position;
    }
    protected void AreaEntered(string areaName, string objectName)
    {
        if (areaName.Equals(_deathRealmToTeleport.name))
        {
            AreaEvent.EnterCorrectArea(_deathRealmToTeleport.name);
        }
    }
    protected override void FinishStep(DialogueInfo dialogue)
    {
        if (dialoguesInfo.name == dialogue.name)
        {
            Finish();
            Destroy(_deathRealmToTeleport);
            _playerToTeleport.gameObject.transform.position = _shoreToTeleport.transform.position;
            AreaEvent.OnAreaEnter -= AreaEntered;
            CutsceneEvents.OnCutsceneFinished -= TeleportPlayer;
            Delete();
        }
    }
}
