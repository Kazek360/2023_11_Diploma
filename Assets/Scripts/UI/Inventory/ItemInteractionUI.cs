using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInteractionUI : MonoBehaviour
{
    [SerializeField] private Image ItemImage;
    [SerializeField] private TextMeshProUGUI ItemName;
    [SerializeField] private TextMeshProUGUI ItemDesc;
    [SerializeField] private Button UseButton, DropButton, MoveButton;
    
    private Item _item;
    private InventorySlot _slot;
    


    private void SetItem(Item item)
    {
        _item = item;
        ItemImage.sprite = item.ItemSprite;
        ItemName.text = item.ItemName;
        ItemDesc.text = item.ItemDesc;
        if (item is EquipmentItem)
        {
            UseButton.enabled = true;
            UseButton.GetComponentInChildren<TextMeshProUGUI>().text = "ZAŁÓŻ";
        }
        else if (item is UsableItem)
        {
            UseButton.enabled = true;
            UseButton.GetComponentInChildren<TextMeshProUGUI>().text = "UŻYJ";
        }
        else
        {
            UseButton.enabled = false;
        }
    }

    public void UseItem()
    {
        if (_item is UsableItem item)
        {
            item.Use();
        } 
        else if (_item is EquipmentItem equipmentItem)
        {
            equipmentItem.Equip();
        }
    }

    public void DropItem()
    {
        _slot.RemoveItem();
    }

    public void MoveItem()
    {
        //TODO: Implement item movement
    }
}
