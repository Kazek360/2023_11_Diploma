using Location;
using Other;
using UnityEngine;

namespace InteractionSystem.Location
{
    public class Teleport : InteractionObject
    {
        [SerializeField] private Transform objectToTeleport;
        
        public override void TriggerInteraction()
        {
            if (disabled)
            {
                return;
            }
            FadeInOutScreen.InvokeOnFadeInOut(() => 
                PlayerPosition.instance.playerTransform.position = objectToTeleport.position);
        }
    }
}