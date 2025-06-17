using System.Collections;
using StorySystems.Dialogues;
using StorySystems.Dialogues.Trigger;
using UnityEngine;

namespace Quests.QuestSteps.DialogueSteps
{
    public class DialogueWithInteractionEnablingStep : DialogueStep
    {
        private GameObject _objectToEnable;
        private InteractionDialogueEntity _trigger;
        
        protected override IEnumerator Start()
        {
            yield return base.Start();
            
            _objectToEnable = GameObject.Find(dialogueInfo.triggerName);
            _trigger  = _objectToEnable.GetComponentInChildren<InteractionDialogueEntity>();
            _trigger.EnableTrigger();
        }
        
        protected override void FinishStep(DialogueInfo dialogue)
        {
            if (dialogueInfo.name == dialogue.name)
            {
                _trigger.DisableTrigger();
                FinishAndDelete();
            } 
        }
        
    }
}