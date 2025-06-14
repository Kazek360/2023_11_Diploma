using Interactions;
using Other;
using UnityEngine;

namespace StorySystems.Dialogues.Trigger
{
    //This class triggers dialogue by interaction
    public class InteractionDialogueEntity : DialogueEntity, IInteractable
    {
        [Header("Interaction")]
        [SerializeField] private GameObject questMarkerSprite;
        
        private FadeInOutObject _interactionKey;
        private bool _disabled;
        
        
        protected override void Awake()
        {
            base.Awake();
            _interactionKey = GetComponentInChildren<FadeInOutObject>();
        }
        
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
                StopAllCoroutines();
                StartCoroutine(_interactionKey.FadeIn());
            }
        }

        public void HideIndicator()
        {
            StopAllCoroutines();
            StartCoroutine(_interactionKey.FadeOut());
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