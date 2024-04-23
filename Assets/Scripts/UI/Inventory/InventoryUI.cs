using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySlotsUI Slots;

    public void OnEnable()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        foreach (InventorySlotUI slot in Slots.GetSlots())
        {
            InventorySlot actualSlot = Inventory.GetInstance().GetSlot(slot.GetCoordinateX(), slot.GetCoordinateY());
            
            slot.UpdateSlot(actualSlot.Item.ItemSprite, actualSlot.Count);
        }
    }
}
