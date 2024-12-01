
using InputSystem;

namespace UI.Interfaces
{
    public class SettingsMenu : UserInterface
    {
        public override UserInterfaceState State => UserInterfaceState.Settings;
        public override void OnEnter()
        {
            base.OnEnter();
            InputManager.Instance.Controlls.Player.Disable();
        }

        public override void OnExit()
        {
            base.OnExit();
            InputManager.Instance.Controlls.Player.Enable();
        }
    }
 
}
