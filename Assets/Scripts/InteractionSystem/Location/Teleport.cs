using Location;
using UnityEngine;

namespace InteractionSystem.Location
{
    public class Teleport : InteractionObject
    {
        [SerializeField] private Transform objectToTeleport;
        
        public override void TriggerInteraction()
        {
            PlayerPosition.instance.playerTransform.position = objectToTeleport.position;
        }
    }
}