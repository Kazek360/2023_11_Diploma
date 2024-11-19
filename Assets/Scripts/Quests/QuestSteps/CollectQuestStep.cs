using Collectibles;
using UnityEngine;

namespace Quests.QuestSteps
{
    public class CollectQuestStep : BaseQuestStep
    {
        [SerializeField] protected int itemID;
        
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
        
        private void IncreaseProgress(int id)
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