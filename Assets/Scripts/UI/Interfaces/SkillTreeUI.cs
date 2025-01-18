using InputSystem;
using TMPro;
using UnityEngine;

namespace UI.Interfaces
{
    public class SkillTreeUI : UserInterface
    {
        public override UserInterfaceState State => UserInterfaceState.SkillTree;
        
        [SerializeField] private TextMeshProUGUI skillPointsValue;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            PlayerResourcesEvents.OnSkillPointsChange += ChangeSkillPointsValue;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            PlayerResourcesEvents.OnSkillPointsChange -= ChangeSkillPointsValue;
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
            skillPointsValue.text = value.ToString();
        }
    }
}
