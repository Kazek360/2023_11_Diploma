using System.Collections.Generic;
using TMPro;
using UI.Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlotsUI : MonoBehaviour
{
    [SerializeField] private InventoryUI InventoryUI;
        
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI experienceText;
    [SerializeField] private Image experienceProgressBar;
    
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
        levelText.text = $"Poziom: {PlayerStats.GetInstance().Level}";
        experienceText.text = $"Punkty doświadczenia: {PlayerStats.GetInstance().CurrentXp}/1000";
        experienceProgressBar.fillAmount = ((float)PlayerStats.GetInstance().CurrentXp) / 1000;
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

    public void ForceUiUpdate()
    {
        InventoryUI.UpdateDisplay();
    }
}
