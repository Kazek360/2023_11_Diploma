using InputSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Interfaces
{
    public class InventoryUI : UserInterface
    {
        public override UserInterfaceState State => UserInterfaceState.Inventory;
        
        [SerializeField] private InventorySlotsUI InventorySlots;
        [SerializeField] private EquipmentSlotsUI EquipmentSlots;
        [SerializeField] private ItemInteractionUI ItemInteraction;
        [SerializeField] private PlayerStatsUI PlayerStatisticks;
        private ToggleGroup _toggleGroup;
        
        public void Start()
        {
            ItemInteraction.ClearInteraction();
            _toggleGroup = gameObject.GetComponent<ToggleGroup>();
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            Time.timeScale = 0;
            InputManager.Instance.Controlls.Player.Disable();
            UpdateDisplay();
            ClearToggle();
        }
        public override void OnExit()
        {
            base.OnExit();
            Time.timeScale = 1;
            InputManager.Instance.Controlls.Player.Enable();
        }
        
        public void UpdateDisplay()
        {
            //ClearToggle();
            InventorySlots.UpdateUI();
            EquipmentSlots.UpdateUI();
            ItemInteraction.UpdateDisplay();
            PlayerStatisticks.UpdateDisplay();
        }

        public void ClearToggle()
        {
            if (_toggleGroup is not null)
            {
                _toggleGroup.SetAllTogglesOff();
            }
        }
    
        public void SelectedSlotChanged(ItemSlot slot)
        {
            ItemInteraction.SetSlot(slot);
        }
    
        public void SlotDeselected()
        {
            ItemInteraction.ClearInteraction();
        }
        
        public void ExitButton()
        {
            UserInterfaceManager.Instance.ChangeState(UserInterfaceState.Exploration);
        }
    }
}
