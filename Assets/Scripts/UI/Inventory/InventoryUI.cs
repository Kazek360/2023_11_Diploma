using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySlotsUI InventorySlots;
    [SerializeField] private EquipmentSlotsUI EquipmentSlots;
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
        InventorySlots.UpdateUI();
        EquipmentSlots.UpdateUI();
    }

    public void SelectedSlotChanged(ItemSlot slot)
    {
        ItemInteraction.SetSlot(slot);
    }

    public void SlotDeselected()
    {
        ItemInteraction.ClearInteraction();
    }
}
