using InputSystem;

namespace UI.Interfaces
{
    public class Map : UserInterface
    {
        public override UserInterfaceState State => UserInterfaceState.Map;
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