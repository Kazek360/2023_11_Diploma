using System;
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
    private InventoryUI _inventoryUI;

    public void Awake()
    {
        _inventoryUI = GetComponentInParent<InventoryUI>();
        Debug.Log("DONE SETTING UP");
    }


    private void SetSlot(InventorySlot slot)
    {
        _slot = slot;
        _item = slot.Item;
        ItemImage.sprite = _item.ItemSprite;
        ItemName.text = _item.ItemName;
        ItemDesc.text = _item.ItemDesc;
        if (_item is EquipmentItem)
        {
            UseButton.enabled = true;
            UseButton.GetComponentInChildren<TextMeshProUGUI>().text = "ZAŁÓŻ";
        }
        else if (_item is UsableItem)
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
        Debug.Log("ITEM USED");
        _inventoryUI.UpdateDisplay();
    }

    public void DropItem()
    {
        _slot.RemoveItem();
        Debug.Log("ITEM DROPPED");
        _inventoryUI.UpdateDisplay();
    }

    public void MoveItem()
    {
        //TODO: Implement item movement
        Debug.Log("ITEM MOVING");
        _inventoryUI.UpdateDisplay();
    }
}
