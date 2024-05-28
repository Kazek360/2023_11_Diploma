using UnityEngine;
using DialogueSystem.Events;
using InputSystem;

namespace DialogueSystem.Managers
{
    //Class storing NPC dialogues and allowing for their execution in a dialogue window.
    public class DialogueTrigger : MonoBehaviour
    {
        [Header("Name")]
        [SerializeField] private string triggerName;
        
        [Header("Interaction")]
        [SerializeField] private GameObject interactionKeyImage;
        [SerializeField] private bool autoTrigger;
        
        private bool _playerInRange;
        private bool _disabled;

        private void OnEnable()
        {
            InputEvents.OnInteractionInput += TriggerDialogueByInteraction;
            AreaEvent.OnCorrectAreaEnter += TriggerDialogueByAreaEnter;
            PathEvents.OnMovementStarted += DisableTrigger;
            PathEvents.OnMovementEnded += EnableTrigger;
        }
        
        private void OnDisable()
        {
            InputEvents.OnInteractionInput -= TriggerDialogueByInteraction;
            AreaEvent.OnCorrectAreaEnter -= TriggerDialogueByAreaEnter;
            PathEvents.OnMovementStarted -= DisableTrigger;
            PathEvents.OnMovementEnded -= EnableTrigger;
        }

        private void Awake()
        {
            if (!autoTrigger)
            {
                interactionKeyImage.SetActive(false); 
            }
            _playerInRange = false;
            _disabled = false;
        }
        
        private void TriggerDialogueByInteraction()
        {
            if (autoTrigger || _disabled || !_playerInRange)
            {
                return;
            }
            DialogueEvents.TriggerDialogue(triggerName);
        }
        
        private void TriggerDialogueByAreaEnter(string areaName)
        {
            if (!areaName.Equals(gameObject.name))
            {
                return;
            }
            DialogueEvents.TriggerDialogue(triggerName);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (autoTrigger)
            {
                AreaEvent.EnterArea(gameObject.name, other.name);
            }
            else 
            {
                if (other.CompareTag("Player"))
                {
                    interactionKeyImage.SetActive(true);
                    _playerInRange = true;   
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (autoTrigger || !other.CompareTag("Player"))
            {
                return;
            }
            interactionKeyImage.SetActive(false);
            _playerInRange = false;
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

