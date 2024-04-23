using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image ItemImage;
    [SerializeField] private TextMeshProUGUI Count;
    [SerializeField] private Toggle Toggle;
    [SerializeField] private int x, y;
    [SerializeField] private InventorySlotsUI InventorySlots;
    

    public int GetCoordinateX()
    {
        return x;
    }
    
    public int GetCoordinateY()
    {
        return y;
    }

    public void UpdateSlot(Item item, int amount)
    {
        if (item is null)
        {
            ItemImage.enabled = false;
            Toggle.interactable = false;
        }
        else
        {
            ItemImage.enabled = true;
            ItemImage.sprite = item.ItemSprite;
            Toggle.interactable = true;
        }
        if (amount == 0)
        {
            Count.enabled = false;
            return;
        }
        Count.enabled = true;
        Count.text = "" + amount;
        
    }

    public void SlotSelected()
    {
        if (Toggle.isOn)
        {
            InventorySlots.SelectedSlotChanged(x, y);
            return;
        }
        InventorySlots.SlotDeselected();
    }
}
