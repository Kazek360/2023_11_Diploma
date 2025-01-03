using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour
{
    [SerializeField] private Image ItemImage;
    [SerializeField] private Toggle Toggle;
    [SerializeField] private EquipmentSlotType SlotType;
    [SerializeField] private EquipmentSlotsUI EquipmentSlots;
    [SerializeField] private Button AddImaginationButton;
    [SerializeField] private Button LowerImaginationButton;
    [SerializeField] private TextMeshProUGUI ImaginationInvested;

    private int _imaginationInvested = 0;
    private string _lastItemId;

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
            ItemImage.sprite = item.ItemSprite(_imaginationInvested);
            Toggle.interactable = true;
        }
        
    }
    
    public void UpdateSlot()
    {
        EquipmentSlot actualSlot = GameConfig.PlayerInventory.GetEquipmentSlot(SlotType);
        LowerImaginationButton.gameObject.SetActive(false);
        ItemScriptableObject item = actualSlot.Item;
        
        if (item is null)
        {
            PlayerStats.GetInstance().FreeImagination = PlayerStats.GetInstance().FreeImagination + _imaginationInvested;
            _imaginationInvested = 0;
            
            ItemImage.enabled = false;
            Toggle.interactable = false;
            
            AddImaginationButton.gameObject.SetActive(false);
            LowerImaginationButton.gameObject.SetActive(false);
            
        }
        else
        {
            ItemImage.enabled = true;
            ItemImage.sprite = item.ItemSprite(_imaginationInvested);
            Toggle.interactable = true;
            if (!_lastItemId.Equals(item.name))
            {
                PlayerStats.GetInstance().FreeImagination = PlayerStats.GetInstance().FreeImagination + _imaginationInvested;
                _imaginationInvested = 0;
                _lastItemId = item.name;
            }
            LowerImaginationButton.gameObject.SetActive(_imaginationInvested != 0);
            AddImaginationButton.gameObject.SetActive(PlayerStats.GetInstance().FreeImagination != 0);
        }
        ImaginationInvested.text = $"{_imaginationInvested}";
    }

    public void SlotSelected()
    {
        if (Toggle.isOn)
        {
            EquipmentSlots.SelectedSlotChanged(SlotType);
            return;
        }
        EquipmentSlots.SlotDeselected();
    }

    public void AddImagination()
    {
        _imaginationInvested++;
        PlayerStats.GetInstance().FreeImagination = PlayerStats.GetInstance().FreeImagination - 1;
        if (PlayerStats.GetInstance().FreeImagination == 0)
        {
            AddImaginationButton.gameObject.SetActive(false);
        }
        LowerImaginationButton.gameObject.SetActive(true);
        GameConfig.PlayerInventory.GetEquipmentSlot(SlotType).ChangeImagination(_imaginationInvested);
        EquipmentSlots.ForceUiUpdate();
    } 
    
    public void LowerImagination()
    {
        _imaginationInvested--;
        PlayerStats.GetInstance().FreeImagination = PlayerStats.GetInstance().FreeImagination + 1;
        if (_imaginationInvested == 0)
        {
            LowerImaginationButton.gameObject.SetActive(false);
        }
        AddImaginationButton.gameObject.SetActive(true);
        GameConfig.PlayerInventory.GetEquipmentSlot(SlotType).ChangeImagination(_imaginationInvested);
        EquipmentSlots.ForceUiUpdate();
    } 
}
