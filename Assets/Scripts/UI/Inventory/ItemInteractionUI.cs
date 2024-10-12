using TMPro;
using UI.Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class ItemInteractionUI : MonoBehaviour
{
    [SerializeField] private Image ItemImage;
    [SerializeField] private TextMeshProUGUI ItemName;
    [SerializeField] private TextMeshProUGUI ItemDesc;
    [SerializeField] private Button UseButton, DropButton, MoveButton;
    
    private ItemSlot _slot;
    private InventoryUI _inventoryUI;

    public void Awake()
    {
        _inventoryUI = GetComponentInParent<InventoryUI>();
    }


    public void SetSlot(ItemSlot slot)
    {
        _slot = slot;
        Item item = slot.Item;
        
        ItemImage.gameObject.SetActive(true);
        ItemName.gameObject.SetActive(true);
        ItemDesc.gameObject.SetActive(true);
        
        UseButton.gameObject.SetActive(true);
        
        ItemImage.sprite = item.ItemSprite;
        ItemName.text = item.ItemName;
        ItemDesc.text = item.ItemDesc();
        if (slot is InventorySlot)
        {
            DropButton.gameObject.SetActive(true);
            //MoveButton.gameObject.SetActive(true);
        
            if (item is EquipmentItem)
            {
                UseButton.GetComponentInChildren<TextMeshProUGUI>().text = "ZAŁÓŻ";
            }
            else if (item is UsableItem)
            {
                UseButton.GetComponentInChildren<TextMeshProUGUI>().text = "UŻYJ";
            }
            else
            {
                UseButton.gameObject.SetActive(false);
            }
        }
        else
        {
            ItemImage.sprite = item.ItemSprite;
            ItemName.text = item.ItemName;
            ItemDesc.text = item.ItemDesc();
        
            UseButton.GetComponentInChildren<TextMeshProUGUI>().text = "ZDEJMIJ";
        }
        
    }

    public void ClearInteraction()
    {
        ItemImage.gameObject.SetActive(false);
        ItemName.gameObject.SetActive(false);
        ItemDesc.gameObject.SetActive(false);
        
        UseButton.gameObject.SetActive(false);
        DropButton.gameObject.SetActive(false);
        MoveButton.gameObject.SetActive(false);
    }

    public void UseItem()
    {
        _slot.Use();
        ClearInteraction();
        _inventoryUI.UpdateDisplay();
    }

    public void DropItem()
    {
        ((InventorySlot)_slot).RemoveItem();
        ClearInteraction();
        _inventoryUI.UpdateDisplay();
    }

    public void MoveItem()
    {
        //TODO: Implement item movement
        _inventoryUI.UpdateDisplay();
    }
}
