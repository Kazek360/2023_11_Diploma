using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour
{
    [SerializeField] private Image ItemImage;
    [SerializeField] private Toggle Toggle;
    [SerializeField] private EquipmentSlotType SlotType;
    [SerializeField] private InventorySlotsUI InventorySlots;
    


    public void EquipItem(Item item)
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
        
    }

    public void SlotSelected()
    {
        
    }
}
