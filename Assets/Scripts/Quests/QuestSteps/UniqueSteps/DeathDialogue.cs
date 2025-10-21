using Quests.QuestSteps.DialogueSteps;
using UnityEngine;

namespace Quests.QuestSteps.UniqueSteps
{
    public class DeathDialogue : DialogueOnStartStep
    {
        private GameObject _shorePoint;
        private GameObject _playerToTeleport;
    
        protected override void OnEnable()
        {
            base.OnEnable();
            _shorePoint = GameObject.Find("ShorePoint");
            _playerToTeleport = GameObject.Find("Player");
        }
    
        protected override void FinishStep(string finishedDialogueTrigger)
        {
            if (triggerName == finishedDialogueTrigger)
            {
                _playerToTeleport.gameObject.transform.position = _shorePoint.transform.position;
                FinishAndDelete();
            } 
        }
    }
}

