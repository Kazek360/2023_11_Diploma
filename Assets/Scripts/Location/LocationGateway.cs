using System;
using System.Collections.Generic;
using Interactions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Location
{
    public class LocationGateway : MonoBehaviour, IInteractable
    {
        public static List<LocationGateway> allGateways = new List<LocationGateway>();
        
        [SerializeField] private string sceneToLoad;
        [SerializeField] private string gatewayID;

        public string GatewayID => gatewayID;

        [SerializeField] private GameObject interactionIndicator;

        private void Awake()
        {
            allGateways.Add(this);
        }

        private void OnDestroy()
        {
            allGateways.Remove(this);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("InteractionArea"))
            {
                InteractionEvents.EnterInteractionArea(this);
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("InteractionArea"))
            {
                InteractionEvents.ExitInteractionArea(this);
            }
        }

        public void TriggerInteraction()
        {
            CharacterPositionManager.Instance.GatewayID = gatewayID;
            SceneManager.LoadScene(sceneToLoad);
        }

        public Vector3 GetCoordinates()
        {
            return transform.position;
        }

        public void ShowIndicator()
        {
            interactionIndicator.SetActive(true);
        }

        public void HideIndicator()
        {
            interactionIndicator.SetActive(false);
        }
    }
}
