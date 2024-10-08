using System.Collections.Generic;
using UI.Interfaces;
using UnityEngine;

public class EquipmentSlotsUI : MonoBehaviour
{
    [SerializeField] private InventoryUI InventoryUI;
    
    private List<EquipmentSlotUI> _slots;
    
    private void OnEnable()
    {
        _slots = new List<EquipmentSlotUI>();
        foreach (Transform child in transform)
        {
            EquipmentSlotUI script = child.GetComponent<EquipmentSlotUI>();
            if (script is not null)
            {
                _slots.Add(script);
            }
        }
    }
    
    public void UpdateUI()
    {
        if (_slots is null)
        {
            _slots = new List<EquipmentSlotUI>();
            foreach (Transform child in transform)
            {
                EquipmentSlotUI script = child.GetComponent<EquipmentSlotUI>();
                if (script is not null)
                {
                    _slots.Add(script);
                }
            }
        }
        foreach (EquipmentSlotUI slot in _slots)
        {
            slot.UpdateSlot();
        }
    }
    
    public List<EquipmentSlotUI> GetSlots()
    {
        return _slots;
    }

    public void SelectedSlotChanged(EquipmentSlotType type)
    {
        InventoryUI.SelectedSlotChanged(GameConfig.PlayerInventory.GetEquipmentSlot(type));
    }
    
    public void SlotDeselected()
    {
        InventoryUI.SlotDeselected();
    }
}
