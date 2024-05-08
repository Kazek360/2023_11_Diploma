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
        
        public static event Action OnInteractionInput;
        public static void InputInteraction()
        {
            OnInteractionInput?.Invoke();
        }
        public static event Action OnDialogueContinueInput;
        public static void InputDialogueContinue()
        {
            OnDialogueContinueInput?.Invoke();
        }
        public static event Action OnCutsceneContinueInput;
        public static void InputCutsceneContinue()
        {
            OnCutsceneContinueInput?.Invoke();
        }
        public static event Action OnQuestLogToggle;
        public static void ToggleQuestLog()
        {
            OnQuestLogToggle?.Invoke();
        }
        public static event Action OnSkillTreeToggle;
        public static void ToggleSkillTree()
        {
            OnSkillTreeToggle?.Invoke();
        }
        public static event Action OnInventoryToggle;
        public static void ToggleInventory()
        {
            OnInventoryToggle?.Invoke();
        }
        public static event Action OnInterfaceClosed;
        public static void CloseInterface()
        {
            OnInterfaceClosed?.Invoke();
        }
        public static event Action OnPauseToggle;
        public static void TogglePause()
        {
            OnPauseToggle?.Invoke();
        }
    }
}
