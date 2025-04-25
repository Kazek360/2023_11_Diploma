using Cutscenes;
using UnityEngine;

namespace Quests.QuestSteps.AreaSteps
{
    public class EnterAreaWithCutsceneStep : BaseQuestStep
    {
        [SerializeField] protected string areaToEnterName;
        private GameObject _area;
        [SerializeField] private CutsceneSO cutsceneToTrigger;
    
        protected override void OnEnable()
        {
            base.OnEnable();
            AreaEvent.OnAreaEnter += TriggerCutscene;
            CutsceneEvents.OnCutsceneFinished += FinishStep;
            _area = GameObject.Find(areaToEnterName);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            AreaEvent.OnAreaEnter -= TriggerCutscene;
            CutsceneEvents.OnCutsceneFinished -= FinishStep;
        }

        private void TriggerCutscene(string areaName, string objectName)
        {
            if (areaName.Equals(areaToEnterName))
            {
                Destroy(_area);
                CutsceneEvents.StartCutscene(cutsceneToTrigger);
            }
        }
    
        private void FinishStep()
        {
            FinishAndDelete();
        }
    }
}


