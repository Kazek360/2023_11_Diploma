using InteractionSystem;
using Other;
using UnityEngine;

namespace StorySystems.Dialogues.Trigger
{
    //This class triggers dialogue by interaction
    public class InteractionDialogueTrigger : MonoBehaviour, IInteractable
    {
        [Header("Interaction")]
        [SerializeField] private GameObject questMarkerSprite;
        [SerializeField] private bool disabledOnStart;

        [Header("Dialogues info")]
        [SerializeField] private string triggerName;
        [SerializeField] private TextAsset dialogue;
        
        private bool _disabled;
        private FadeInOutObject _interactionKey;
        
        private void Awake()
        {
            _interactionKey = GetComponentInChildren<FadeInOutObject>();
            _disabled = disabledOnStart;
        }
        
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
            if (_disabled)
            {
                return;
            }
            DialogueEvent.InitializeDialogue();
            DialogueEvent.StartDialogue(triggerName, dialogue);
        }
        
        public Vector3 GetCoordinates()
        {
            return transform.position;
        }

        public void ShowIndicator()
        {
            if (_disabled)
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
            if (_disabled)
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