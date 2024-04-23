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
    }


    public void SetSlot(InventorySlot slot)
    {
        _slot = slot;
        _item = slot.Item;
        
        ItemImage.gameObject.SetActive(true);
        ItemName.gameObject.SetActive(true);
        ItemDesc.gameObject.SetActive(true);
        
        UseButton.gameObject.SetActive(true);
        DropButton.gameObject.SetActive(true);
        MoveButton.gameObject.SetActive(true);
        
        ItemImage.sprite = _item.ItemSprite;
        ItemName.text = _item.ItemName;
        ItemDesc.text = _item.ItemDesc;
        if (_item is EquipmentItem)
        {
            UseButton.GetComponentInChildren<TextMeshProUGUI>().text = "ZAŁÓŻ";
        }
        else if (_item is UsableItem)
        {
            UseButton.GetComponentInChildren<TextMeshProUGUI>().text = "UŻYJ";
        }
        else
        {
            UseButton.gameObject.SetActive(false);
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
        if (_item is UsableItem item)
        {
            item.Use();
        } 
        else if (_item is EquipmentItem equipmentItem)
        {
            equipmentItem.Equip();
        }
        _inventoryUI.UpdateDisplay();
    }

    public void DropItem()
    {
        _slot.RemoveItem();
        ClearInteraction();
        _inventoryUI.UpdateDisplay();
    }

    public void MoveItem()
    {
        //TODO: Implement item movement
        _inventoryUI.UpdateDisplay();
    }
}
