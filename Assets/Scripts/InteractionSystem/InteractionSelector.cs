using System.Collections.Generic;
using StorySystems.Dialogues;
using UnityEngine;

namespace InteractionSystem
{
    public class InteractionSelector : MonoBehaviour
    {
        private List<IInteractable> _availableInteractions = new List<IInteractable>();
        private IInteractable _closestInteraction;
        private bool _enabled;

        private void OnEnable()
        {
            InteractionEvents.OnInteractionAreaEnter += AddToAvailableInteractions;
            InteractionEvents.OnInteractionAreaExit += RemoveFromAvailableInteractions;
            InteractionEvents.OnInteractionRemove += RemoveFromAvailableInteractions;
            InteractionEvents.OnInteractionTrigger += TriggerClosestInteraction;
            DialogueEvent.OnDialogueInitialized += DisableInteraction;
            DialogueEvent.OnDialogueFinished += EnableInteraction;
        }
        private void OnDisable()
        {
            InteractionEvents.OnInteractionAreaEnter -= AddToAvailableInteractions;
            InteractionEvents.OnInteractionAreaExit -= RemoveFromAvailableInteractions;
            InteractionEvents.OnInteractionRemove -= RemoveFromAvailableInteractions;
            InteractionEvents.OnInteractionTrigger -= TriggerClosestInteraction;
            DialogueEvent.OnDialogueInitialized -= DisableInteraction;
            DialogueEvent.OnDialogueFinished -= EnableInteraction;
        }

        private void Start()
        {
            _enabled = true;
        }
        
        private void Update()
        {
            if (_enabled)
            {
                CalculateClosestInteraction();
            }
        }

        private void TriggerClosestInteraction()
        {
            if (_closestInteraction != null)
            {
                _closestInteraction.TriggerInteraction();
            }
        }
        
        private void AddToAvailableInteractions(IInteractable newInteractionObject)
        {
            _availableInteractions.Add(newInteractionObject);
        }
        private void RemoveFromAvailableInteractions(IInteractable newInteractionObject)
        {
            _availableInteractions.Remove(newInteractionObject);
        }

        private void CalculateClosestInteraction()
        {
            if (_availableInteractions.Count == 0)
            {
                if (_closestInteraction == null)
                {
                    return;
                }
                _closestInteraction.HideIndicator();
                _closestInteraction = null;
                return;
            }

            var playerPosition = transform.position;
            float minDistance = 10000;
            var currentClosestInteraction = _availableInteractions[0];
                
            foreach (var interaction in _availableInteractions)
            {
                var interactionPosition = interaction.GetCoordinates();
                var distance = Vector3.Distance(playerPosition, interactionPosition);
                if (distance > minDistance)
                {
                    continue;
                }
                minDistance = distance;
                currentClosestInteraction = interaction;
            }

            ChangeClosestInteraction(currentClosestInteraction);
        }

        private void ChangeClosestInteraction(IInteractable newClosestInteraction)
        {
            if (_closestInteraction != null)
            {
                if (_closestInteraction == newClosestInteraction)
                {
                    return;
                }
                _closestInteraction.HideIndicator();
            }
            _closestInteraction = newClosestInteraction;
            _closestInteraction.ShowIndicator();
        }

        private void DisableInteraction()
        {
            _enabled = false;
            if (_closestInteraction != null)
            {
                _closestInteraction.HideIndicator();
            }
        }

        private void EnableInteraction()
        {
            _enabled = true;
        }
    }
}
