using UnityEngine;
using StorySystems.Dialogues;

namespace Quests.QuestSteps.DialogueSteps
{
    public class AreaDialogueStep : DialogueStep
    {
        [SerializeField] protected string areaToEnterName;
        protected override void OnEnable()
        {
            base.OnEnable();
            AreaEvent.OnAreaEnter += AreaEntered;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            AreaEvent.OnAreaEnter -= AreaEntered;
        }
    
        protected virtual void AreaEntered(string areaName, string objectName)
        {
            if (areaName.Equals(areaToEnterName))
            {
                DialogueManager.Instance.TriggerDialogue(dialogueInfo.triggerName);
            }
        }
    
        protected override void FinishStep(DialogueInfo dialogue)
        {
            if (dialogueInfo.name == dialogue.name)
            {
                FinishAndDelete();
            } 
        }
    }
}

