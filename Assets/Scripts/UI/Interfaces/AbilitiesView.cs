using InputSystem;
using UnityEngine;

namespace UI.Interfaces
{
    public class AbilitiesView : UserInterface
    {
        public override GameState State => GameState.Abilities;

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
    }
}
