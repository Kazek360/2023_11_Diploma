using System.Collections;
using StorySystems.Dialogues;
using StorySystems.Dialogues.Trigger;
using UnityEngine;

namespace Quests.QuestSteps.DialogueSteps
{
    public class DialogueWithInteractionEnablingStep : DialogueStep
    {
        private GameObject _objectToEnable;
        private InteractionDialogueTrigger _trigger;
        
        protected void Start()
        {
            _objectToEnable = GameObject.Find(triggerName);
            _trigger  = _objectToEnable.GetComponentInChildren<InteractionDialogueTrigger>();
            _trigger.EnableTrigger();
        }
        
        protected override void FinishStep(string finishedDialogueTrigger)
        {
            if (triggerName == finishedDialogueTrigger)
            {
                _trigger.DisableTrigger();
                FinishAndDelete();
            } 
        }
        
    }
}