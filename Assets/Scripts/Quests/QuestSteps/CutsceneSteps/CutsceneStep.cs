using Cutscenes;
using UnityEngine;

namespace Quests.QuestSteps.CutsceneSteps
{
    public class CutsceneStep : BaseQuestStep
    {
        [SerializeField] private CutsceneSO cutscene;
    
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
    
        private void FinishStep(CutsceneSO cutsceneSo)
        {
            if (cutscene.name.Equals(cutsceneSo.name))
            {
                FinishAndDelete();
            }
        }
    }
}


