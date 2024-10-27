using Cutscenes;
using DialogueSystem.Events;
using Interactions;
using Shared;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class InputManager : Singleton<InputManager>, 
        PlayerControlls.IPlayerActions, PlayerControlls.IStoryActions, PlayerControlls.IUIActions
    {
        private PlayerControlls _controlls;
        public PlayerControlls Controlls => _controlls;
        
        private void OnDisable()
        {
            _controlls.Player.RemoveCallbacks(this);
            _controlls.Story.RemoveCallbacks(this);
            _controlls.UI.RemoveCallbacks(this);
        }
        
        protected override void Awake()
        {
            PrepareInputSystem();
        }

        private void FixedUpdate()
        {
            if (_controlls.Player.enabled)
            {
                InputEvents.InputMovement(_controlls.Player.Movement.ReadValue<Vector2>());
            }
        }
        
        private void PrepareInputSystem()
        {
            _controlls = new PlayerControlls();
            
            _controlls.Player.SetCallbacks(this);
            _controlls.Story.SetCallbacks(this);
            _controlls.UI.SetCallbacks(this);
            
            _controlls.Player.Enable();
            _controlls.UI.Enable();

            Time.timeScale = 1;
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            
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
                InteractionEvents.TriggerInteraction();
            }
        }

        public void OnCenterCamera(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                CameraController.CenterCamera();
                //Jakiś Event
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
