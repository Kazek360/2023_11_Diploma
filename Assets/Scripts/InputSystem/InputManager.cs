using Cutscenes;
using Dialogue;
using Interactions;
using Shared;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class InputManager : Singleton<InputManager>, 
        PlayerControlls.IPlayerActions, PlayerControlls.IStoryActions, PlayerControlls.IUIActions, 
        PlayerControlls.IMapActions
    {
        private PlayerControlls _controlls;
        public PlayerControlls Controlls => _controlls;
        
        private void OnDisable()
        {
            _controlls.Player.RemoveCallbacks(this);
            _controlls.Story.RemoveCallbacks(this);
            _controlls.Map.RemoveCallbacks(this);
            _controlls.UI.RemoveCallbacks(this);
        }
        
        protected override void Awake()
        {
            PrepareInputSystem();
        }
        
        private void PrepareInputSystem()
        {
            _controlls = new PlayerControlls();
            
            _controlls.Player.SetCallbacks(this);
            _controlls.Story.SetCallbacks(this);
            _controlls.UI.SetCallbacks(this);
            _controlls.Map.SetCallbacks(this);
            
            _controlls.Player.Enable();
            _controlls.UI.Enable();

            Time.timeScale = 1;
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                InputEvents.InputMovement(_controlls.Player.Movement.ReadValue<Vector2>());
                CameraController.InvokeOnCameraFollow();
            }
            if (context.canceled)
            {
                InputEvents.InvokeOnMovementStop();
            }
        }
        
        public void OnSprint(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                InputEvents.InputSprint(true, _controlls.Player.Movement.ReadValue<Vector2>());
            }
            if (context.canceled)
            {
                InputEvents.InputSprint(false, _controlls.Player.Movement.ReadValue<Vector2>());
            }
        }

        public void OnMouseMovement(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                InputEvents.InvokeOnMouseMovement();
            }
        }

        public void OnCameraDragging(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                CameraController.InvokeOnCameraDragged();
            }
            if (context.canceled)
            {
                CameraController.InvokeOnCameraDragStop();
            }
        }
        public void OnCameraZooming(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                CameraController.InvokeOnCameraZooming(context.ReadValue<Vector2>());
            }
        }
        
        public void OnInteraction(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                InteractionEvents.TriggerInteraction();
            }
        }

        public void OnCenterCamera(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                CameraController.InvokeOnCameraCentered();
            }
        }

        public void OnSkip(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                switch (UserInterfaceManager.Instance.CurrentStateType)
                {
                    case UserInterfaceState.Dialogue:
                        DialogueEvents.ContinueDialogue();
                        break;
                    case UserInterfaceState.Cutscene:
                        CutsceneEvents.SkipCutscene();
                        break;
                }
            }
        }
        
        public void OnToggleQuestLog(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                switch (UserInterfaceManager.Instance.CurrentStateType)
                {
                    case UserInterfaceState.Exploration:
                        UserInterfaceManager.Instance.ChangeState(UserInterfaceState.Quests);
                        break;
                    case UserInterfaceState.Quests:
                        UserInterfaceManager.Instance.ChangeState(UserInterfaceState.Exploration);  
                        break;
                }
            }
        }
        
        public void OnToggleSkillTree(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                switch (UserInterfaceManager.Instance.CurrentStateType)
                {
                    case UserInterfaceState.Exploration:
                        UserInterfaceManager.Instance.ChangeState(UserInterfaceState.SkillTree);
                        break;
                    case UserInterfaceState.SkillTree:
                        UserInterfaceManager.Instance.ChangeState(UserInterfaceState.Exploration);  
                        break;
                }
            }
        }

        public void OnToggleAbilitiesView(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                switch (UserInterfaceManager.Instance.CurrentStateType)
                {
                    case UserInterfaceState.Exploration:
                        UserInterfaceManager.Instance.ChangeState(UserInterfaceState.Abilities);
                        break;
                    case UserInterfaceState.Abilities:
                        UserInterfaceManager.Instance.ChangeState(UserInterfaceState.Exploration);  
                        break;
                }
            }
        }
        
        public void OnToggleInventory(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                switch (UserInterfaceManager.Instance.CurrentStateType)
                {
                    case UserInterfaceState.Exploration:
                        UserInterfaceManager.Instance.ChangeState(UserInterfaceState.Inventory);
                        break;
                    case UserInterfaceState.Inventory:
                        UserInterfaceManager.Instance.ChangeState(UserInterfaceState.Exploration);  
                        break;
                }
            }
        }
        public void OnToggleMap(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                switch (UserInterfaceManager.Instance.CurrentStateType)
                {
                    case UserInterfaceState.Exploration:
                        UserInterfaceManager.Instance.ChangeState(UserInterfaceState.Map);
                        break;
                    case UserInterfaceState.Map:
                        UserInterfaceManager.Instance.ChangeState(UserInterfaceState.Exploration);  
                        break;
                }
            }
        }

        public void OnMapDragging(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                MapCameraController.InvokeOnMapDragging();
            }

            if (context.canceled)
            {
                MapCameraController.InvokeOnMapDraggingStop();
            }
        }
        
        public void OnMapZooming(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                MapCameraController.InvokeOnMapZooming(context.ReadValue<Vector2>());
            }
        }
        
        public void OnToggleEscape(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                switch (UserInterfaceManager.Instance.CurrentStateType)
                {
                    case UserInterfaceState.Exploration or UserInterfaceState.LoadGame or UserInterfaceState.SaveGame 
                        or UserInterfaceState.Combat:
                        UserInterfaceManager.Instance.ChangeState(UserInterfaceState.PauseMenu);
                        break;
                    case UserInterfaceState.PauseMenu or UserInterfaceState.Quests or UserInterfaceState.SkillTree 
                        or UserInterfaceState.Abilities or UserInterfaceState.Inventory:
                        UserInterfaceManager.Instance.ChangeState(UserInterfaceManager.Instance.PreviousState);
                        break;
                }
            }
        }
    }
}
