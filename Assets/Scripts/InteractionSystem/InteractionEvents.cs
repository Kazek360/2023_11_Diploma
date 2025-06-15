using System;

namespace InteractionSystem
{
    public static class InteractionEvents
    {
        public static event Action<IInteractable> OnInteractionAreaEnter;
        public static void EnterInteractionArea(IInteractable interactionObject)
        {
            OnInteractionAreaEnter?.Invoke(interactionObject);
        }
        public static event Action<IInteractable> OnInteractionAreaExit;
        public static void ExitInteractionArea(IInteractable interactionObject)
        {
            OnInteractionAreaExit?.Invoke(interactionObject);
        }
        public static event Action<IInteractable> OnInteractionRemove;
        public static void RemoveInteraction(IInteractable interactionObject)
        {
            OnInteractionRemove?.Invoke(interactionObject);
        }
        public static event Action OnInteractionTrigger;
        public static void TriggerInteraction()
        {
            OnInteractionTrigger?.Invoke();
        }
    }
}

