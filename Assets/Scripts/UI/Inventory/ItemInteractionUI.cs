using System;
using System.Text;
using Skill;
using Skill.Managers;
using TMPro;
using UI.Interfaces;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class ItemInteractionUI : MonoBehaviour
{
    [SerializeField] private Image ItemImage;
    [SerializeField] private TextMeshProUGUI ItemName;
    [SerializeField] private TextMeshProUGUI ItemDesc;
    [SerializeField] private TextMeshProUGUI ImaginationLevelDisplay;
    [SerializeField] private Button UseButton, DropButton, MoveButton, EquipButton, UnequipButton;
    [SerializeField] private Button AddImaginationButton, LowerImaginationButton;
    
    
    private ItemSlot _slot;
    private InventoryUI _inventoryUI;
    
    private int _imaginationInvested = 0;

    public void Awake()
    {
        _inventoryUI = GetComponentInParent<InventoryUI>();
    }

    public void UpdateDisplay()
    {
        if (_slot is not null)
        {
            SetSlot(_slot);
        }
        else
        {
            ClearInteraction();
        }
    }


    public void SetSlot(ItemSlot slot)
    {
        _slot = slot;
        ItemScriptableObject item = slot.Item;
        
        ItemImage.gameObject.SetActive(true);
        ItemName.gameObject.SetActive(true);
        ItemDesc.gameObject.SetActive(true);
        
        UseButton.gameObject.SetActive(true);
        
        ItemImage.sprite = item.ItemSprite();
        ItemName.text = item.itemName;
        ItemDesc.text = item.ItemDesc();
        if (slot is InventorySlot)
        {
            AddImaginationButton.gameObject.SetActive(false);
            LowerImaginationButton.gameObject.SetActive(false);
            ImaginationLevelDisplay.gameObject.SetActive(false);
            
            UseButton.interactable = true;
            DropButton.interactable = true;
            DropButton.gameObject.SetActive(true);
            //MoveButton.gameObject.SetActive(true);
        
            if (item.itemType == ItemScriptableObject.ItemType.EQUIPMENT)
            {
                UseButton.gameObject.SetActive(false);
                EquipButton.gameObject.SetActive(true);
            }
            else if (item.itemType == ItemScriptableObject.ItemType.CONSUMABLE)
            {
                EquipButton.gameObject.SetActive(false);
                UseButton.gameObject.SetActive(true);
            }
            else
            {
                UseButton.gameObject.SetActive(false);
            }
        }
        else
        {
            if (IsImaginationEnabled())
            {
                AddImaginationButton.gameObject.SetActive(true);
                LowerImaginationButton.gameObject.SetActive(true);
                LowerImaginationButton.interactable = true;
                ImaginationLevelDisplay.gameObject.SetActive(true);
                AddImaginationButton.interactable = true;
                _imaginationInvested = ((EquipmentSlot)_slot).ImaginationInvested;
                ImaginationLevelDisplay.text = $"{_imaginationInvested}";
            }
            
            if (_imaginationInvested == 0)
            {
                LowerImaginationButton.interactable = false;
            }
            if (PlayerStats.GetInstance().FreeImagination == 0 || _imaginationInvested >= MaxImaginationInvestment())
            {
                AddImaginationButton.interactable = false;
            }
            
            UseButton.gameObject.SetActive(false);
            DropButton.gameObject.SetActive(false);
            EquipButton.gameObject.SetActive(false);
            UnequipButton.gameObject.SetActive(true);
            
            ItemImage.sprite = item.ItemSprite();
            ItemName.text = item.itemName;

            StringBuilder descFinal = new StringBuilder();
            if (((EquipmentSlot)_slot).ModifierVal > 0)
            {
                descFinal.Append($"FAWORYTYZM: PODSTAWOWE STATYSTYKI PODNIESIONE O {(int)(((EquipmentSlot)_slot).ModifierVal * 100)}%\n\n");
            }

            if (SkillTreeManager.GetNodeByName("IMAGINATION_5_1").state == SkillTreeNode.State.Activated)
            {
                descFinal.Append($"PAMIĄTKI: PODSTAWOWE STATYSTYKI PODNIESIONE O 30%\n\n");
            }
            descFinal.Append(item.ItemDesc());
            
            ItemDesc.text = descFinal.ToString();
            
            
        
            //UseButton.GetComponentInChildren<TextMeshProUGUI>().text = "ZDEJMIJ";
        }
        
    }

    public void ClearInteraction()
    {
        _slot = null;
        AddImaginationButton.gameObject.SetActive(false);
        LowerImaginationButton.gameObject.SetActive(false);
        ImaginationLevelDisplay.gameObject.SetActive(false);
        ItemImage.gameObject.SetActive(false);
        ItemName.gameObject.SetActive(false);
        ItemDesc.gameObject.SetActive(false);
        
        UseButton.gameObject.SetActive(true);
        DropButton.gameObject.SetActive(true);
        UseButton.interactable = false;
        DropButton.interactable = false;
        
        MoveButton.gameObject.SetActive(false);
        EquipButton.gameObject.SetActive(false);
        UnequipButton.gameObject.SetActive(false);
    }

    public void UseItem()
    {
        _slot.Use();
        ClearInteraction();
        _inventoryUI.UpdateDisplay();
        _inventoryUI.ClearToggle();
    }

    public void DropItem()
    {
        ((InventorySlot)_slot).RemoveItem();
        ClearInteraction();
        _inventoryUI.UpdateDisplay();
        _inventoryUI.ClearToggle();
    }

    public void MoveItem()
    {
        //TODO: Implement item movement
        _inventoryUI.UpdateDisplay();
        _inventoryUI.ClearToggle();
    }
    
    public void AddImagination()
    {
        _imaginationInvested++;
        ImaginationLevelDisplay.text = $"{_imaginationInvested}";
        PlayerStats.GetInstance().FreeImagination = PlayerStats.GetInstance().FreeImagination - 1;
        if (PlayerStats.GetInstance().FreeImagination == 0 || _imaginationInvested >= MaxImaginationInvestment())
        {
            AddImaginationButton.interactable = false;
        }
        if (_imaginationInvested > 0)
        {
            LowerImaginationButton.interactable = true;
        }
        
        GameConfig.PlayerInventory.GetEquipmentSlot(((EquipmentSlot)_slot).SlotType).ChangeImagination(_imaginationInvested);
        //EquipmentSlots.ForceUiUpdate();
        
        _inventoryUI.UpdateDisplay();
    } 
    
    public void LowerImagination()
    {
        _imaginationInvested--;
        ImaginationLevelDisplay.text = $"{_imaginationInvested}";
        PlayerStats.GetInstance().FreeImagination = PlayerStats.GetInstance().FreeImagination + 1;
        if (_imaginationInvested <= 0)
        {
            LowerImaginationButton.interactable = false;
        }
        if (PlayerStats.GetInstance().FreeImagination > 0 && _imaginationInvested < MaxImaginationInvestment())
        {
            AddImaginationButton.interactable = true;
        }
        
        GameConfig.PlayerInventory.GetEquipmentSlot(((EquipmentSlot)_slot).SlotType).ChangeImagination(_imaginationInvested);
        //EquipmentSlots.ForceUiUpdate();
        
        _inventoryUI.UpdateDisplay();
    }

    private int MaxImaginationInvestment()
    {
        if (SkillTreeManager.GetNodeByName("IMAGINATION_3_3").state == SkillTreeNode.State.Activated)
        {
            return 3;
        }
        if (SkillTreeManager.GetNodeByName("IMAGINATION_2_3").state == SkillTreeNode.State.Activated)
        {
            return 2;
        }
        if (SkillTreeManager.GetNodeByName("IMAGINATION_1_1").state == SkillTreeNode.State.Activated)
        {
            return 1;
        }
        return 0;
    }

    private bool IsImaginationEnabled()
    {
        return SkillTreeManager.GetNodeByName("IMAGINATION_1_1").state == SkillTreeNode.State.Activated;
    }
}
