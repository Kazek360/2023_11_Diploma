using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySlotsUI InventorySlots;
    [SerializeField] private EquipmentSlotsUI EquipmentSlots;
    [SerializeField] private ItemInteractionUI ItemInteraction;
    [SerializeField] private PlayerStatsUI PlayerStatisticks;
    private ToggleGroup _toggleGroup;
    
    public void Start()
    {
        UpdateDisplay();
        ItemInteraction.ClearInteraction();
        _toggleGroup = gameObject.GetComponent<ToggleGroup>();
    }

    public void OnEnable()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        if (_toggleGroup is not null)
        {
            _toggleGroup.SetAllTogglesOff();
        }
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
