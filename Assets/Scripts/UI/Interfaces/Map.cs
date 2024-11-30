using InputSystem;

namespace UI.Interfaces
{
    public class Map : UserInterface
    {
        public override UserInterfaceState State => UserInterfaceState.Map;
        
        public override void OnEnter()
        {
            MapCameraController.InvokeOnMapModeEnter();
            base.OnEnter();
            InputManager.Instance.Controlls.Player.Disable();
            InputManager.Instance.Controlls.Map.Enable();
        }
        
        public override void OnExit()
        {
            base.OnExit();
            InputManager.Instance.Controlls.Player.Enable();
            InputManager.Instance.Controlls.Map.Disable();
        }
    }
}