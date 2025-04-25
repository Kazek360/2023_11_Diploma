using Dialogues;
using Quests.QuestSteps.DialogueSteps;
using UnityEngine;

namespace Quests.QuestSteps.UniqueSteps
{
    public class DeathDialogue : DialogueStep
    {
        private GameObject _shorePoint;
        private GameObject _playerToTeleport;
    
        protected override void OnEnable()
        {
            base.OnEnable();
            _shorePoint = GameObject.Find("ShorePoint");
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
                _playerToTeleport.gameObject.transform.position = _shorePoint.transform.position;
                FinishAndDelete();
            } 
        }
    }
}

