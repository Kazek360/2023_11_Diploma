using InteractionSystem;
using UnityEngine;

namespace StorySystems.Cutscenes
{
    public class CutsceneTrigger : InteractionObject
    {
        [SerializeField] private Cutscene cutscene;
    
        public override void TriggerInteraction()
        {
            CutsceneEvents.StartCutscene(cutscene);
        }
    }
}