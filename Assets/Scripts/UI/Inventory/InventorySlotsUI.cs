using System.Collections.Generic;
using UI.Interfaces;
using UnityEngine;

public class InventorySlotsUI : MonoBehaviour
{
    [SerializeField] private InventoryUI InventoryUI;

    private List<InventorySlotUI> _slots;
    
    private void OnEnable()
    {
        _slots = new List<InventorySlotUI>();
        foreach (Transform child in transform)
        {
            InventorySlotUI script = child.GetComponent<InventorySlotUI>();
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
            _slots = new List<InventorySlotUI>();
            foreach (Transform child in transform)
            {
                InventorySlotUI script = child.GetComponent<InventorySlotUI>();
                if (script is not null)
                {
                    _slots.Add(script);
                }
            }
        }
        foreach (InventorySlotUI slot in _slots)
        {
            slot.UpdateSlot();
        }
    }


    public List<InventorySlotUI> GetSlots()
    {
        return _slots;
    }

    public void SelectedSlotChanged(int x, int y)
    {
        InventoryUI.SelectedSlotChanged(GameConfig.PlayerInventory.GetSlot(x, y));
    }
    
    public void SlotDeselected()
    {
        InventoryUI.SlotDeselected();
    }
}
