using System.Collections;
using System.Collections.Generic;
using InputSystem;
using UI;
using UI.Interfaces;
using UnityEngine;

namespace Tutorials
{
    public class TutorialUI : UserInterface
    {
        public override UserInterfaceState State => UserInterfaceState.Tutorial;
        
        public override void OnEnter()
        {
            base.OnEnter();
            Time.timeScale = 0;
            InputManager.Instance.Controlls.Player.Disable();
            InputManager.Instance.Controlls.UI.Disable();
            
        }
        public override void OnExit()
        {
            base.OnExit();
            Time.timeScale = 1;
            InputManager.Instance.Controlls.Player.Enable();
            InputManager.Instance.Controlls.UI.Enable();
        }
    }

}
