using InputSystem;

namespace UI.Interfaces
{
    public class LoadingScreen : UserInterface
    {
        public override UserInterfaceState State => UserInterfaceState.LoadingScreen;
        
        public override void OnEnter()
        {
            base.OnEnter();
            InputManager.Instance.Controlls.Player.Disable();
            InputManager.Instance.Controlls.UI.Disable();
        }
        
        public override void OnExit()
        {
            base.OnExit();
            InputManager.Instance.Controlls.Player.Enable();
            InputManager.Instance.Controlls.UI.Enable();
        }
    }
}
