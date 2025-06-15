using UnityEngine;

namespace InteractionSystem.Collectibles
{
    /**
     * Controller for an item that can be picked up by a player
     */
    public class Collectible : InteractionObject, ITooltippable
    {
        [SerializeField] private string itemId;
        [SerializeField] private int amount;
        
        private SpriteRenderer _spriteRenderer;
        private ItemScriptableObject _itemInfo;
        
        public void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _itemInfo = ItemLibrary.Instance.GetItem(itemId);
            _spriteRenderer.sprite = _itemInfo.ItemSprite();
        }
        
        public override void TriggerInteraction()
        {
            CollectibleEvents.PickupCollectible(itemId);
            InteractionEvents.RemoveInteraction(this);
            GameConfig.PlayerInventory.AddItem(itemId, amount);
            gameObject.SetActive(false);
        }
        
        public string GetTooltipHeader()
        {
            return _itemInfo.itemName;
        }
    
        public string GetTooltipDesc()
        {
            return _itemInfo.ItemDesc();
        }
    }
}
