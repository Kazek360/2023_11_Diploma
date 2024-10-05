using Interactions;
using UnityEngine;

namespace Collectibles
{
    /**
     * Controller for an item that can be picked up by a player
     */
    public class PickUpItem : MonoBehaviour, ITooltippable, IInteractable
    {
        [SerializeField] private int itemId;
        [SerializeField] private int amount;
        [SerializeField] private GameObject pickupIndicatorSprite;
        
        private SpriteRenderer _spriteRenderer;
        
        public void OnEnable()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = ItemLibrary.GetInstance().GetItem(itemId).ItemSprite;
        }
    
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                InteractionEvents.EnterInteractionArea(this);
            }
        }
    
        public void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                InteractionEvents.ExitInteractionArea(this);
                
            }
        }
    
        public string GetTooltipHeader()
        {
            return ItemLibrary.GetInstance().GetItem(itemId).ItemName;
        }
    
        public string GetTooltipDesc()
        {
            return ItemLibrary.GetInstance().GetItem(itemId).ItemDesc();
        }

        public void TriggerInteraction()
        {
            CollectibleEvents.PickupCollectible(itemId);
            InteractionEvents.RemoveInteraction(this);
            GameConfig.PlayerInventory.AddItem(itemId, amount);
            gameObject.SetActive(false);
        }

        public Vector3 GetCoordinates()
        {
            return transform.position;
        }

        public void ShowIndicator()
        {
            pickupIndicatorSprite.SetActive(true);
        }

        public void HideIndicator()
        {
            pickupIndicatorSprite.SetActive(false);
        }
    }
}
