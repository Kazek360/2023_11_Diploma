using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotsUI : MonoBehaviour
{
    private List<InventorySlotUI> _slots;
    private void Awake()
    {
        _slots = new List<InventorySlotUI>();
        foreach (GameObject child in transform)
        {
            InventorySlotUI script = child.GetComponent<InventorySlotUI>();
            if (script is not null)
            {
                _slots.Add(script);
            }
        }
    }

    public InventorySlotUI GetSlot(int x, int y)
    {
        foreach (InventorySlotUI slot in _slots)
        {
            if (slot.GetCoordinateX() == x && slot.GetCoordinateY() == y)
            {
                return slot;
            }
        }
    }
}
