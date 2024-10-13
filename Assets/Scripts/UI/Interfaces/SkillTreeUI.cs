using InputSystem;
using TMPro;
using UnityEngine;

namespace UI.Interfaces
{
    public class SkillTreeUI : UserInterface
    {
        public override GameState State => GameState.SkillTree;
        
        [SerializeField] private TextMeshProUGUI skillPointsValue;
    
        public void OnEnable()
        {
            PlayerResourcesEvents.OnSkillPointsChange += ChangeSkillPointsValue;
        }
        public void OnDisable()
        {
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
