using System;
using Other;
using UnityEngine;

namespace InteractionSystem.Unique
{
    public class MillGear : MonoBehaviour, IInteractable
    {
        [SerializeField] private FadeInOutObject sprite;
        [SerializeField] private FadeInOutObject interactionKey;
        
        protected bool disabled;
        
        public static event Action OnGearFixed;

        public static void InvokeOnGearFixed()
        {
            OnGearFixed?.Invoke();
        }

        private void OnEnable()
        {
            OnGearFixed += FixGear;
        }

        private void OnDisable()
        {
            OnGearFixed -= FixGear;
        }

        protected void Awake()
        {
            disabled = true;
            var spriteRenderer = GetComponent<SpriteRenderer>();
            var color = spriteRenderer.color;
            color.a = 0;
            spriteRenderer.color = color;
        }
        
        public void TriggerInteraction()
        {
            if (disabled)
            {
                return;
            }
            InteractionEvents.EnterInteraction(gameObject.name);
        }
        
        private void FixGear()
        {
            DisableInteraction();
            StartCoroutine(sprite.FadeIn());
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
