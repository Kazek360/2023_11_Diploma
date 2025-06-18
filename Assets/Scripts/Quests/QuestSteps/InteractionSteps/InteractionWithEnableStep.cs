using InteractionSystem.Unique;
using UnityEngine;

namespace Quests.QuestSteps.InteractionSteps
{
    public class InteractionWithEnableStep : InteractionStep
    {
        private GameObject _objectToEnable;
        
        private void Start()
        {
            _objectToEnable = GameObject.Find(objectToInteractName);
            var interaction  = _objectToEnable.GetComponent<MillGear>();
            interaction.EnableInteraction();
        }
    }
}