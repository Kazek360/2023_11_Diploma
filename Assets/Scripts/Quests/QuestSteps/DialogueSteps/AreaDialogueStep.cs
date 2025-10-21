using StorySystems.Dialogues;
using UnityEngine;

namespace Quests.QuestSteps.DialogueSteps
{
    public class AreaDialogueStep : DialogueStep
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            DialogueEvent.OnDialogueAreaEnter += AreaEntered;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            DialogueEvent.OnDialogueAreaEnter -= AreaEntered;
        }
    
        protected virtual void AreaEntered(string areaDialogueTrigger, TextAsset dialogue, string collisionName)
        {
            if (triggerName.Equals(areaDialogueTrigger))
            {
                DialogueEvent.InitializeDialogue();
                DialogueEvent.StartDialogue(areaDialogueTrigger, dialogue);
            }
        }
    }
}

