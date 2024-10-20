using InputSystem;
using UnityEngine;

namespace UI.Interfaces
{
    public class CutsceneUI : UserInterface
    {
        public override UserInterfaceState State => UserInterfaceState.Cutscene;
        
        public override void OnEnter()
        {
            base.OnEnter();
            Time.timeScale = 0;
            InputManager.Instance.Controlls.Player.Disable();
            InputManager.Instance.Controlls.UI.Disable();
            InputManager.Instance.Controlls.Story.Enable();   
        }
        
        public override void OnExit()
        {
            base.OnExit();
            Time.timeScale = 1;
            InputManager.Instance.Controlls.Player.Enable();
            InputManager.Instance.Controlls.UI.Enable();
            InputManager.Instance.Controlls.Story.Disable();   
        }
    }
}