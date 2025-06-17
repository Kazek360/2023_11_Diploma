using StorySystems.Cutscenes;
using UnityEngine;

namespace Quests.QuestSteps.CutsceneSteps
{
    public class CutsceneStep : BaseQuestStep
    {
        [SerializeField] private Cutscene cutscene;
    
        protected override void OnEnable()
        {
            base.OnEnable();
            CutsceneEvents.OnCutsceneFinished += FinishStep;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            CutsceneEvents.OnCutsceneFinished -= FinishStep;
        }
    
        private void FinishStep(Cutscene cutscene)
        {
            if (this.cutscene.name.Equals(cutscene.name))
            {
                FinishAndDelete();
            }
        }
    }
}


