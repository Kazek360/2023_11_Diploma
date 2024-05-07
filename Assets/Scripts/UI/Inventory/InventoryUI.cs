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
    [SerializeField] private PlayerStatsUI PlayerStatisticks;

    public void OnEnable()
    {
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
        PlayerStatisticks.UpdateDisplay();
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
