using InteractionSystem.Collectibles;
using Quests.QuestSteps.DialogueSteps;
using StorySystems.Dialogues;
using UnityEngine;

namespace Quests.QuestSteps.UniqueSteps
{
    public class CollectRuneStep : DialogueStep
    {
        [SerializeField] protected string itemID;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            CollectibleEvents.OnCollectiblePickup += ProgressQuest;
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            CollectibleEvents.OnCollectiblePickup -= ProgressQuest;
        }
        
        private void ProgressQuest(string id)
        {
            if (itemID == id)
            {
                DialogueManager.Instance.TriggerDialogue("RunePrologue");       
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