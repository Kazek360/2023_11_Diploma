using Other;
using UnityEngine;

namespace InteractionSystem
{
    public class InteractionObject : MonoBehaviour, IInteractable
    {
        [SerializeField] private bool disabledOnStart;
        protected bool disabled;
        protected FadeInOutObject interactionKey;
        
        protected virtual void Awake()
        {
            interactionKey = GetComponentInChildren<FadeInOutObject>();
            disabled = disabledOnStart;
        }

        public virtual void TriggerInteraction()
        {
            if (disabled || !gameObject.activeSelf)
            {
                return;
            }
            Debug.Log($"Interacted with object: {gameObject.name}");
        }
        
        public Vector3 GetCoordinates()
        {
            return transform.position;
        }

        public virtual void ShowIndicator()
        {
            if (disabled || !gameObject.activeSelf)
            {
                return;
            }
            StopAllCoroutines();
            StartCoroutine(interactionKey.FadeIn());
        }

        public void HideIndicator()
        {
            if (!gameObject.activeSelf)
            {
                return;
            }
            StopAllCoroutines();
            StartCoroutine(interactionKey.FadeOut());
        }
        
        protected void OnTriggerEnter2D(Collider2D other)
        {
            if (disabled || !gameObject.activeSelf)
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
        
        public void EnableInteraction()
        {
            disabled = false;
        }
        public void DisableInteraction()
        {
            disabled = true;
        }
    }
}