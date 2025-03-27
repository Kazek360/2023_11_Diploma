using InputSystem;
using TMPro;
using UnityEngine;

namespace UI.Interfaces
{
    public class SkillTreeUI : UserInterface
    {
        public override UserInterfaceState State => UserInterfaceState.SkillTree;
        
        [SerializeField] private TextMeshProUGUI skillPointsText;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            PlayerResourcesEvents.OnSkillPointsValueChanged += ChangeSkillPointsValue;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            PlayerResourcesEvents.OnSkillPointsValueChanged -= ChangeSkillPointsValue;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Time.timeScale = 0;
            InputManager.Instance.Controlls.Player.Disable();
        }
        public override void OnExit()
        {
            base.OnExit();
            Time.timeScale = 1;
            InputManager.Instance.Controlls.Player.Enable();
        }
    
        private void ChangeSkillPointsValue(int value)
        {
            skillPointsText.text = value.ToString();
        }
    }
}
