using System.Collections.Generic;
using UnityEngine;

namespace Interactions
{
    public class InteractionSelector : MonoBehaviour
    {
        private List<IInteractable> _availableInteractions = new List<IInteractable>();
        private IInteractable _closestInteraction;

        private void OnEnable()
        {
            InteractionEvents.OnInteractionAreaEnter += AddToAvailableInteractions;
            InteractionEvents.OnInteractionAreaExit += RemoveFromAvailableInteractions;
            InteractionEvents.OnInteractionRemove += RemoveFromAvailableInteractions;
            InteractionEvents.OnInteractionTrigger += TriggerClosestInteraction;
        }
        private void OnDisable()
        {
            InteractionEvents.OnInteractionAreaEnter -= AddToAvailableInteractions;
            InteractionEvents.OnInteractionAreaExit -= RemoveFromAvailableInteractions;
            InteractionEvents.OnInteractionRemove -= RemoveFromAvailableInteractions;
            InteractionEvents.OnInteractionTrigger -= TriggerClosestInteraction;
        }

        private void Update()
        {
            CalculateClosestInteraction();
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
                    _closestInteraction.HideIndicator();
                    _closestInteraction = currentClosestInteraction;
                    _closestInteraction.ShowIndicator();
                }
                else
                {
                    _closestInteraction.HideIndicator();
                    _closestInteraction = _availableInteractions[0];
                    _closestInteraction.ShowIndicator();
                }
            }
            else if (_closestInteraction is not null)
            {
                _closestInteraction.HideIndicator();
                _closestInteraction = null;
            }
        }
    }
}
