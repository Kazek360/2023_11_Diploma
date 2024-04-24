using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        InventoryUI.SelectedSlotChanged(Inventory.GetInstance().GetSlot(x, y));
    }
    
    public void SlotDeselected()
    {
        InventoryUI.SlotDeselected();
    }
}
