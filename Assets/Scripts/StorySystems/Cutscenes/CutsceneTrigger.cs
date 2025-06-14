using Cutscenes;
using Interactions;
using UnityEngine;

namespace StorySystems.Cutscenes
{
    public class CutsceneTrigger : InteractionObject
    {
        [SerializeField] private CutsceneSO cutscene;
    
        public override void TriggerInteraction()
        {
            CutsceneEvents.StartCutscene(cutscene);
        }
    }
}