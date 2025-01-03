using Interactions;
using UnityEngine;

namespace Collectibles
{
    /**
     * Controller for an item that can be picked up by a player
     */
    public class Collectible : MonoBehaviour, ITooltippable, IInteractable
    {
        [SerializeField] private string itemId;
        [SerializeField] private int amount;
        [SerializeField] private GameObject pickupIndicatorSprite;
        
        private SpriteRenderer _spriteRenderer;
        private ItemScriptableObject _itemInfo;
        
        public void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _itemInfo = ItemLibrary.Instance.GetItem(itemId);
            _spriteRenderer.sprite = _itemInfo.ItemSprite();
        }
    
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("InteractionArea"))
            {
                InteractionEvents.EnterInteractionArea(this);
            }
        }
    
        public void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("InteractionArea"))
            {
                InteractionEvents.ExitInteractionArea(this);
            }
        }
    
        public string GetTooltipHeader()
        {
            return _itemInfo.itemName;
        }
    
        public string GetTooltipDesc()
        {
            return _itemInfo.ItemDesc();
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
