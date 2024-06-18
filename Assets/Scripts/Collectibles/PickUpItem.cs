using UnityEngine;

namespace Collectibles
{
    /**
     * Controller for an item that can be picked up by a player
     */
    public class PickUpItem : MonoBehaviour, ITooltippable
    {
        [SerializeField] private int itemId;
        [SerializeField] private int amount;
        [SerializeField] private GameObject pickupIndicatorSprite;
        
        private SpriteRenderer _spriteRenderer;
        private bool _inRange;
        
        public void OnEnable()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = ItemLibrary.GetInstance().GetItem(itemId).ItemSprite;
        }
    
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.E) && _inRange)
            {
                GameConfig.PlayerInventory.AddItem(itemId, amount);
                gameObject.SetActive(false);
            }
        }
    
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _inRange = true;
                pickupIndicatorSprite.SetActive(true);
            }
        }
    
        public void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _inRange = false;
                pickupIndicatorSprite.SetActive(false);
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
    }

}
