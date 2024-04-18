using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class InputManager : MonoBehaviour, PlayerControlls.IPlayerActions
    {
        private PlayerControlls _controlls;
        private void Awake()
        {
            PrepareInputSystem();
        }

        private void FixedUpdate()
        {
            InputEvents.InputMovement(_controlls.Player.Movement.ReadValue<Vector2>());
        }
        
        private void PrepareInputSystem()
        {
            _controlls = new PlayerControlls();
            _controlls.Player.SetCallbacks(this);
            _controlls.Player.Enable();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                InputEvents.InputSprint(true);
            }
            if (context.canceled)
            {
                InputEvents.InputSprint(false);
            }
        }

        public void OnInteraction(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                InputEvents.InputInteraction();
            }
        }
    }
}
