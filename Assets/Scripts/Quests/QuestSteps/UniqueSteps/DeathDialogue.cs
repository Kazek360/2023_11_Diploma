using Cutscenes;
using Dialogue;
using Quests.QuestSteps.DialogueQuestStep;
using UnityEngine;

public class DeathDialogue : DialoguesQuestStep
{
    private GameObject _deathRealmToTeleport;
    private GameObject _shoreToTeleport;
    private GameObject _playerToTeleport;
    protected override void OnEnable()
    {
        base.OnEnable();
        AreaEvent.OnAreaEnter += CheckIfCorrectAreaEntered;
        CutsceneEvents.OnCutsceneFinished += TeleportPlayerToDeathRealm;
        _deathRealmToTeleport = GameObject.Find("DeathArea");
        _shoreToTeleport = GameObject.Find("ShoreArea");
        _playerToTeleport = GameObject.Find("Player");
    }
    
    private void TeleportPlayerToDeathRealm()
    {
        _playerToTeleport.gameObject.transform.position = _deathRealmToTeleport.transform.position;
    }
    private void TeleportPlayerToShore()
    {
        _playerToTeleport.gameObject.transform.position = _shoreToTeleport.transform.position;
    }
    private void CheckIfCorrectAreaEntered(string areaName, string objectName)
    {
        switch (currentDialogueIndex)
        {
            case 0:
                if (areaName.Equals(_deathRealmToTeleport.name))
                {
                    AreaEvent.EnterCorrectArea(_deathRealmToTeleport.name);
                }
                break;
            case 1:
                if (areaName.Equals(_shoreToTeleport.name))
                {
                    AreaEvent.EnterCorrectArea(_shoreToTeleport.name);
                }
                break;
        }
    }
    
    protected override void NextDialogue()
    {
        currentDialogueIndex++;
        if (currentDialogueIndex == dialogues.Count)
        {
            AreaEvent.OnAreaEnter -= CheckIfCorrectAreaEntered;
            CutsceneEvents.OnCutsceneFinished -= TeleportPlayerToDeathRealm;
            FinishAndDelete();
        }
        else
        {
            DialogueEvents.InvokeOnDialogueUnlocked(dialogues[currentDialogueIndex].triggerName, 
                dialogues[currentDialogueIndex].name);
            TeleportPlayerToShore();
        }
    }
}
