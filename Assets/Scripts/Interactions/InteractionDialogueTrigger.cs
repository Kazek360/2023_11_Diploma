using DialogueSystem.Events;
using UnityEngine;

namespace Interactions
{
    //This class triggers dialogue by interaction
    public class InteractionDialogueTrigger : MonoBehaviour, IInteractable
    {
        [Header("Name")]
        [SerializeField] private string triggerName;
        
        [Header("Interaction")]
        [SerializeField] private GameObject pickupIndicatorSprite;
        
        private bool _disabled;
        
        private void OnEnable()
        {
            PathEvents.OnMovementStarted += DisableTrigger;
            PathEvents.OnMovementEnded += EnableTrigger;
        }
        
        private void OnDisable()
        {
            PathEvents.OnMovementStarted -= DisableTrigger;
            PathEvents.OnMovementEnded -= EnableTrigger;
        }
        
        
        public void TriggerInteraction()
        {
            if (!_disabled)
            {
                DialogueEvents.TriggerDialogue(triggerName);
            }
        }

        public Vector3 GetCoordinates()
        {
            return transform.position;
        }

        public void ShowIndicator()
        {
            pickupIndicatorSprite.SetActive(true);
        }

        public void HideIndicator()
        {
            pickupIndicatorSprite.SetActive(false);
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
        
        private void DisableTrigger()
        {
            _disabled = true;
        }
        private void EnableTrigger()
        {
            _disabled = false;
        }
        
    }
}