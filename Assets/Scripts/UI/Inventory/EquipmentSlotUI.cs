using System;
using System.Collections;
using System.Collections.Generic;
using Skill;
using Skill.Managers;
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
    [SerializeField] private Toggle FavoriteToggle;

    private int _imaginationInvested = 0;
    private string _lastItemId = "";
    private bool _isFavourite = false;

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
        
        FavoriteToggle.gameObject.SetActive(false);
        
        
        EquipmentSlot actualSlot = GameConfig.PlayerInventory.GetEquipmentSlot(SlotType);
        LowerImaginationButton.gameObject.SetActive(false);
        ItemScriptableObject item = actualSlot.Item;
        
        if (item is null)
        {
            PlayerStats.GetInstance().FreeImagination = PlayerStats.GetInstance().FreeImagination + _imaginationInvested;
            _imaginationInvested = 0;
            _isFavourite = false;
            GameConfig.PlayerInventory.GetEquipmentSlot(SlotType).SetImagination(_imaginationInvested);
            
            ItemImage.enabled = false;
            Toggle.interactable = false;
            
            AddImaginationButton.gameObject.SetActive(false);
            LowerImaginationButton.gameObject.SetActive(false);
            
        }
        else
        {
            if (SkillTreeManager.GetNodeByName("IMAGINATION_2_2").state == SkillTreeNode.State.Activated)
            {
                FavoriteToggle.gameObject.SetActive(true);
            }
            
            _imaginationInvested = GameConfig.PlayerInventory.GetEquipmentSlot(SlotType).ImaginationInvested;
            ItemImage.enabled = true;
            ItemImage.sprite = item.ItemSprite(_imaginationInvested);
            Toggle.interactable = true;
            if (!_lastItemId.Equals(item.name))
            {
                PlayerStats.GetInstance().FreeImagination = PlayerStats.GetInstance().FreeImagination + _imaginationInvested;
                _imaginationInvested = 0;
                GameConfig.PlayerInventory.GetEquipmentSlot(SlotType).SetImagination(_imaginationInvested);
                _lastItemId = item.name;
            }
            //LowerImaginationButton.gameObject.SetActive(_imaginationInvested != 0);
            //AddImaginationButton.gameObject.SetActive(PlayerStats.GetInstance().FreeImagination != 0);
        }
        ImaginationInvested.text = $"{_imaginationInvested}";
        if (IsImaginationEnabled() && item is not null)
        {
            ImaginationInvested.gameObject.SetActive(true);
        }
        else
        {
            ImaginationInvested.gameObject.SetActive(false);
        }
        FavoriteToggle.isOn = _isFavourite;
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
    
    private bool IsImaginationEnabled()
    {
        return SkillTreeManager.GetNodeByName("IMAGINATION_1_1").state == SkillTreeNode.State.Activated;
    }

    public int ImaginationInvested1 => _imaginationInvested;

    public void OnFavorite()
    {
        _isFavourite = FavoriteToggle.isOn;
        var slot = GameConfig.PlayerInventory.GetEquipmentSlot(SlotType);
        if (FavoriteToggle.isOn)
        {
            if (SkillTreeManager.GetNodeByName("IMAGINATION_4_2").state == SkillTreeNode.State.Activated) //60
            {
                slot.ApplyFavorite(1f);
            }
            else if (SkillTreeManager.GetNodeByName("IMAGINATION_3_2").state == SkillTreeNode.State.Activated) //40
            {
                slot.ApplyFavorite(0.66f);
            }
            else if (SkillTreeManager.GetNodeByName("IMAGINATION_2_2").state == SkillTreeNode.State.Activated) //20
            {
                slot.ApplyFavorite(0.33f);
            }
        }
        else if (slot.Item is null)
        {
            //DO NOTHING
        }
        else if (_lastItemId.Equals(slot.Item.name) || _lastItemId.Equals(""))
        {
            slot.ApplyFavorite(0);
            slot.Reset();
        }
        EquipmentSlots.ForceUiUpdate();
    }

    public bool IsFavourite => _isFavourite;
}
