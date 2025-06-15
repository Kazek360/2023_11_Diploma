using Other;
using UnityEngine;

namespace InteractionSystem
{
    public class InteractionObject : MonoBehaviour, IInteractable
    {
        private FadeInOutObject _interactionKey;
        
        private void Awake()
        {
            _interactionKey = GetComponentInChildren<FadeInOutObject>();
        }

        public virtual void TriggerInteraction()
        {
            Debug.Log($"Interacted with object: {gameObject.name}");
        }
        
        public Vector3 GetCoordinates()
        {
            return transform.position;
        }

        public void ShowIndicator()
        {
            StopCoroutine(_interactionKey.FadeOut());
            StartCoroutine(_interactionKey.FadeIn());
        }

        public void HideIndicator()
        {
            StopCoroutine(_interactionKey.FadeIn());
            StartCoroutine(_interactionKey.FadeOut());
        }
        
        protected void OnTriggerEnter2D(Collider2D other)
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
    }
}