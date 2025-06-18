using InteractionSystem.Collectibles;
using UnityEngine;

namespace Quests.QuestSteps.InteractionSteps
{
    public class CollectItemStep : BaseQuestStep
    {
        [SerializeField] protected string itemID;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            CollectibleEvents.OnCollectiblePickup += IncreaseProgress;
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            CollectibleEvents.OnCollectiblePickup -= IncreaseProgress;
        }
        
        protected virtual void IncreaseProgress(string id)
        {
            if (itemID == id)
            {
                progress++;
                QuestEvents.ProgressQuestStep(quest.info.name);
                if (progress == quest.currentStepProgress)
                {
                    FinishAndDelete();
                }
            } 
        }
    }
}