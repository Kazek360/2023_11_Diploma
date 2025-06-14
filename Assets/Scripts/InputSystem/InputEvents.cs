using System;
using UnityEngine;

namespace InputSystem
{
    public static class InputEvents
    {
        public static event Action<bool, Vector2> OnSprintInput;
        public static void InputSprint(bool isPlayerSprinting, Vector2 movementVector)
        {
            OnSprintInput?.Invoke(isPlayerSprinting, movementVector);
        }
        public static event Action<Vector2> OnMovementInput;
        public static void InputMovement(Vector2 movementVector)
        {
            OnMovementInput?.Invoke(movementVector);
        }
        public static event Action OnMovementStop;
        public static void InvokeOnMovementStop()
        {
            OnMovementStop?.Invoke();
        }
        
        public static event Action OnMouseMovement;
        public static void InvokeOnMouseMovement()
        {
            OnMouseMovement?.Invoke();
        }
        
        public static event Action<bool> OnInputChanged;
        public static void InvokeOnInputChanged(bool value)
        {
            OnInputChanged?.Invoke(value);
        }
    }
}
