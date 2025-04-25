using Interactions;
using UnityEngine;

namespace Dialogues.Trigger
{
    //This class triggers dialogue by interaction
    public class InteractionDialogueEntity : DialogueEntity, IInteractable
    {
        [Header("Interaction")]
        [SerializeField] private GameObject interactionKeySprite;
        [SerializeField] private GameObject questMarkerSprite;
        
        private bool _disabled;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            PathEvents.OnMovementStarted += DisableTrigger;
            PathEvents.OnMovementEnded += EnableTrigger;
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            PathEvents.OnMovementStarted -= DisableTrigger;
            PathEvents.OnMovementEnded -= EnableTrigger;
        }
        
        public override void ChangeCurrentDialogue()
        {
             base.ChangeCurrentDialogue();
             if (questMarkerSprite == null)
             {
                 return;
             }
             if (currentDialogue.info.dialogueType != DialogueInfo.Type.Repeatable)
             {
                 questMarkerSprite.SetActive(true);
             }
             else
             {
                 questMarkerSprite.SetActive(false);
             }
        }
        
        public void TriggerInteraction()
        {
            if (!_disabled)
            {
                StartDialogue();
            }
        }
        
        public Vector3 GetCoordinates()
        {
            return transform.position;
        }

        public void ShowIndicator()
        {
            if (!_disabled)
            {
                interactionKeySprite.SetActive(true);  
            }
        }

        public void HideIndicator()
        {
            interactionKeySprite.SetActive(false);
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