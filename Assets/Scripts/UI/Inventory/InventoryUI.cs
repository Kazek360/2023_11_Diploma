using System;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySlotsUI InventorySlots;
    [SerializeField] private EquipmentSlotsUI EquipmentSlots;
    [SerializeField] private ItemInteractionUI ItemInteraction;
    [SerializeField] private PlayerStatsUI PlayerStatisticks;
    
    public void Start()
    {
        UpdateDisplay();
        ItemInteraction.ClearInteraction();
    }

    public void OnEnable()
    {
        UpdateDisplay();
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
