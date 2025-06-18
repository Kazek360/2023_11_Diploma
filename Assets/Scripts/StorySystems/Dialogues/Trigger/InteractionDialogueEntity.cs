using InteractionSystem;
using Other;
using UnityEngine;

namespace StorySystems.Dialogues.Trigger
{
    //This class triggers dialogue by interaction
    public class InteractionDialogueEntity : DialogueEntity, IInteractable
    {
        [Header("Interaction")]
        [SerializeField] private GameObject questMarkerSprite;
        [SerializeField] private bool disabledOnStart;
        
        private bool _disabled;
        private FadeInOutObject _interactionKey;
        
        protected override void Awake()
        {
            base.Awake();
            _interactionKey = GetComponentInChildren<FadeInOutObject>();
            _disabled = disabledOnStart;
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
             if (questMarkerSprite == null || currentDialogue == null)
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
            if (_disabled || currentDialogue == null)
            {
                return;
            }
            StartDialogue();
        }
        
        public Vector3 GetCoordinates()
        {
            return transform.position;
        }

        public void ShowIndicator()
        {
            if (_disabled || currentDialogue == null)
            {
                return;
            }
            StopAllCoroutines();
            StartCoroutine(_interactionKey.FadeIn());
        }

        public void HideIndicator()
        {
            StopAllCoroutines();
            StartCoroutine(_interactionKey.FadeOut());
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_disabled || currentDialogue == null)
            {
                return;
            }
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
        
        public void DisableTrigger()
        {
            _disabled = true;
        }
        public void EnableTrigger()
        {
            _disabled = false;
        }
    }
}