using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySlotsUI Slots;
    [SerializeField] private ItemInteractionUI ItemInteraction;

    public void OnEnable()
    {
        Inventory.GetInstance();
    }

    public void Start()
    {
        UpdateDisplay();
        ItemInteraction.ClearInteraction();
    }

    public void UpdateDisplay()
    {
        foreach (InventorySlotUI slot in Slots.GetSlots())
        {
            InventorySlot actualSlot = Inventory.GetInstance().GetSlot(slot.GetCoordinateX(), slot.GetCoordinateY());
            slot.UpdateSlot(actualSlot.Item, actualSlot.Count);
        }
    }

    public void SelectedSlotChanged(InventorySlot slot)
    {
        ItemInteraction.SetSlot(slot);
    }

    public void SlotDeselected()
    {
        ItemInteraction.ClearInteraction();
    }
}
