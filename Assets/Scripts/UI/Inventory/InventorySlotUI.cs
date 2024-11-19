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
    
    public void UpdateSlot()
    {
        InventorySlot actualSlot = GameConfig.PlayerInventory.GetSlot(x, y);
        ItemScriptableObject item = actualSlot.Item;
        if (item is null)
        {
            ItemImage.enabled = false;
            Toggle.interactable = false;
        }
        else
        {
            ItemImage.enabled = true;
            ItemImage.sprite = item.ItemSprite();
            Toggle.interactable = true;
        }
        if (actualSlot.Count == 0)
        {
            Count.enabled = false;
            return;
        }
        Count.enabled = true;
        Count.text = "" + actualSlot.Count;
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
