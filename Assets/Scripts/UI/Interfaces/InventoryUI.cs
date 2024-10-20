using InputSystem;
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
        }
        public override void OnExit()
        {
            base.OnExit();
            Time.timeScale = 1;
            InputManager.Instance.Controlls.Player.Enable();
        }
        
        public void UpdateDisplay()
        {
            if (_toggleGroup is not null)
            {
                _toggleGroup.SetAllTogglesOff();
            }
            InventorySlots.UpdateUI();
            EquipmentSlots.UpdateUI();
            PlayerStatisticks.UpdateDisplay();
        }
    
        public void SelectedSlotChanged(ItemSlot slot)
        {
            ItemInteraction.SetSlot(slot);
        }
    
        public void SlotDeselected()
        {
            ItemInteraction.ClearInteraction();
        }
    }
}
