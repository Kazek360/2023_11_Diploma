using Dialogues;
using UnityEngine;

public class DeathDialogue : DialogueQuestStep
{
    private GameObject _shoreToTeleport;
    private GameObject _playerToTeleport;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        _shoreToTeleport = GameObject.Find("ShorePoint");
        _playerToTeleport = GameObject.Find("Player");
    }

    private void Start()
    {
        DialogueManager.Instance.TriggerDialogue("DeathPrologue");
    }
    
    protected override void FinishStep(DialogueInfo dialogue)
    {
        if (dialoguesInfo.name == dialogue.name)
        {
            _playerToTeleport.gameObject.transform.position = _shoreToTeleport.transform.position;
            FinishAndDelete();
        } 
    }
}
