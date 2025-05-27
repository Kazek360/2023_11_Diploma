using TMPro;
using UI.Interfaces;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class ItemInteractionUI : MonoBehaviour
{
    [SerializeField] private Image ItemImage;
    [SerializeField] private TextMeshProUGUI ItemName;
    [SerializeField] private TextMeshProUGUI ItemDesc;
    [SerializeField] private Button UseButton, DropButton, MoveButton, EquipButton, UnequipButton;
    
    
    private ItemSlot _slot;
    private InventoryUI _inventoryUI;

    public void Awake()
    {
        _inventoryUI = GetComponentInParent<InventoryUI>();
    }


    public void SetSlot(ItemSlot slot)
    {
        _slot = slot;
        ItemScriptableObject item = slot.Item;
        
        ItemImage.gameObject.SetActive(true);
        ItemName.gameObject.SetActive(true);
        ItemDesc.gameObject.SetActive(true);
        
        UseButton.gameObject.SetActive(true);
        
        ItemImage.sprite = item.ItemSprite();
        ItemName.text = item.itemName;
        ItemDesc.text = item.ItemDesc();
        if (slot is InventorySlot)
        {
            UseButton.interactable = true;
            DropButton.interactable = true;
            DropButton.gameObject.SetActive(true);
            //MoveButton.gameObject.SetActive(true);
        
            if (item.itemType == ItemScriptableObject.ItemType.EQUIPMENT)
            {
                UseButton.gameObject.SetActive(false);
                EquipButton.gameObject.SetActive(true);
            }
            else if (item.itemType == ItemScriptableObject.ItemType.CONSUMABLE)
            {
                EquipButton.gameObject.SetActive(false);
                UseButton.gameObject.SetActive(true);
            }
            else
            {
                UseButton.gameObject.SetActive(false);
            }
        }
        else
        {
            UseButton.gameObject.SetActive(false);
            DropButton.gameObject.SetActive(false);
            EquipButton.gameObject.SetActive(false);
            UnequipButton.gameObject.SetActive(true);
            
            ItemImage.sprite = item.ItemSprite();
            ItemName.text = item.itemName;
            ItemDesc.text = item.ItemDesc();
            
            
        
            //UseButton.GetComponentInChildren<TextMeshProUGUI>().text = "ZDEJMIJ";
        }
        
    }

    public void ClearInteraction()
    {
        ItemImage.gameObject.SetActive(false);
        ItemName.gameObject.SetActive(false);
        ItemDesc.gameObject.SetActive(false);
        
        UseButton.gameObject.SetActive(true);
        DropButton.gameObject.SetActive(true);
        UseButton.interactable = false;
        DropButton.interactable = false;
        
        MoveButton.gameObject.SetActive(false);
        EquipButton.gameObject.SetActive(false);
        UnequipButton.gameObject.SetActive(false);
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
