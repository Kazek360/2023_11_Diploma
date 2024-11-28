using System.Collections.Generic;
using Dialogue;
using UnityEngine;

namespace Interactions
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
            DialogueEvents.OnDialogueStarted += DisableInteraction;
            DialogueEvents.OnDialogueFinished += EnableInteraction;
        }
        private void OnDisable()
        {
            InteractionEvents.OnInteractionAreaEnter -= AddToAvailableInteractions;
            InteractionEvents.OnInteractionAreaExit -= RemoveFromAvailableInteractions;
            InteractionEvents.OnInteractionRemove -= RemoveFromAvailableInteractions;
            InteractionEvents.OnInteractionTrigger -= TriggerClosestInteraction;
            DialogueEvents.OnDialogueStarted -= DisableInteraction;
            DialogueEvents.OnDialogueFinished -= EnableInteraction;
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
            if (_availableInteractions.Count > 0)
            {
                if (_closestInteraction is null)
                {
                    _closestInteraction = _availableInteractions[0];
                }

                if (_availableInteractions.Count > 1)
                {
                    var playerPosition = transform.position;
                    float minDistance = 10000;
                    var currentClosestInteraction = _availableInteractions[0];
                
                    foreach (var availableInteraction in _availableInteractions)
                    {
                        var interactionObjectPosition = availableInteraction.GetCoordinates();
                        var distance = Vector3.Distance(playerPosition, interactionObjectPosition);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            currentClosestInteraction = availableInteraction;
                        }
                    }

                    ChangeClosestInteraction(currentClosestInteraction);
                }
                else
                {
                    ChangeClosestInteraction(_availableInteractions[0]);
                }
            }
            else if (_closestInteraction != null)
            {
                _closestInteraction.HideIndicator();
                _closestInteraction = null;
            }
        }

        private void ChangeClosestInteraction(IInteractable newClosestInteraction)
        {
            _closestInteraction.HideIndicator();
            _closestInteraction = newClosestInteraction;
            _closestInteraction.ShowIndicator();
        }

        private void DisableInteraction(DialogueInfo info)
        {
            _enabled = false;
            if (_closestInteraction != null)
            {
                _closestInteraction.HideIndicator();
            }
        }

        private void EnableInteraction(DialogueInfo info)
        {
            _enabled = true;
        }
    }
}
