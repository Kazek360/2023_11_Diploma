using System;
using UnityEngine;

namespace InputSystem
{
    public static class InputEvents
    {
        public static event Action<bool> OnSprintInput;
        public static void InputSprint(bool isPlayerSprinting)
        {
            OnSprintInput?.Invoke(isPlayerSprinting);
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
    }
}
