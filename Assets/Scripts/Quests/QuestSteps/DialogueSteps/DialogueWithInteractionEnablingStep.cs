using System.Collections;
using StorySystems.Dialogues.Trigger;
using UnityEngine;

namespace Quests.QuestSteps.DialogueSteps
{
    public class DialogueWithInteractionEnablingStep : DialogueStep
    {
        private GameObject _objectToEnable;
        
        protected override IEnumerator Start()
        {
            yield return base.Start();
            
            _objectToEnable = GameObject.Find(dialogueInfo.triggerName);
            var trigger = _objectToEnable.GetComponentInChildren<InteractionDialogueEntity>();
            trigger.EnableTrigger();
        }
    }
}