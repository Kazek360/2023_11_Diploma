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
            InputManager.Instance.DisableInputs();
            InputManager.Instance.Controlls.Map.Enable();
        }
        
        public override void OnExit()
        {
            base.OnExit();
            InputManager.Instance.EnableInputs();
            InputManager.Instance.Controlls.Map.Disable();
        }
    }
}