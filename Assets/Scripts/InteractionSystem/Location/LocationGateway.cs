using System.Collections.Generic;
using Location;
using UnityEngine;

namespace InteractionSystem.Location
{
    public class LocationGateway : InteractionObject
    {
        public static List<LocationGateway> allGateways = new List<LocationGateway>();
        public static string IdToUse;
        
        [SerializeField] private string sceneToLoad;
        [SerializeField] private string gatewayID;

        public string GatewayID => gatewayID;

        [SerializeField] private GameObject interactionIndicator;

        protected override void Awake()
        {
            base.Awake();
            allGateways.Add(this);
        }

        private void OnDestroy()
        {
            allGateways.Remove(this);
        }

        public override void TriggerInteraction()
        {
            IdToUse = gatewayID;
            SceneLoader.InvokeOnSceneChanged(sceneToLoad);
        }
    }
}
