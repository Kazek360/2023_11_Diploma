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
        
        [SerializeField] private bool disabledOnStart;
        private bool _disabled;
        
        public void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _itemInfo = ItemLibrary.Instance.GetItem(itemId);
            _spriteRenderer.sprite = _itemInfo.ItemSprite();
            if (disabledOnStart)
            {
                _disabled = true;
            }
        }
        
        public override void TriggerInteraction()
        {
            if (!_disabled)
            {
                CollectibleEvents.PickupCollectible(itemId);
                InteractionEvents.RemoveInteraction(this);
                GameConfig.PlayerInventory.AddItem(itemId, amount);
                gameObject.SetActive(false);
            }
        }
        
        public override void ShowIndicator()
        {
            if (!_disabled)
            {
                StopAllCoroutines();
                StartCoroutine(_interactionKey.FadeIn()); 
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
        
        public void EnableInteraction()
        {
            _disabled = false;
        }
    }
}
