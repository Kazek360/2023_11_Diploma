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
                    case GameState.Dialogue:
                        DialogueEvents.ContinueDialogue();
                        break;
                    case GameState.Cutscene:
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
                    case GameState.Exploration:
                        UserInterfaceManager.Instance.ChangeState(GameState.Quests);
                        break;
                    case GameState.Quests:
                        UserInterfaceManager.Instance.ChangeState(GameState.Exploration);  
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
                    case GameState.Exploration:
                        UserInterfaceManager.Instance.ChangeState(GameState.SkillTree);
                        break;
                    case GameState.SkillTree:
                        UserInterfaceManager.Instance.ChangeState(GameState.Exploration);  
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
                    case GameState.Exploration:
                        UserInterfaceManager.Instance.ChangeState(GameState.Abilities);
                        break;
                    case GameState.Abilities:
                        UserInterfaceManager.Instance.ChangeState(GameState.Exploration);  
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
                    case GameState.Exploration:
                        UserInterfaceManager.Instance.ChangeState(GameState.Inventory);
                        break;
                    case GameState.Inventory:
                        UserInterfaceManager.Instance.ChangeState(GameState.Exploration);  
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
                    case GameState.Exploration or GameState.LoadGame or GameState.SaveGame:
                        UserInterfaceManager.Instance.ChangeState(GameState.PauseMenu);
                        break;
                    case GameState.PauseMenu or GameState.Quests or GameState.SkillTree or GameState.Abilities 
                        or GameState.Inventory:
                        UserInterfaceManager.Instance.ChangeState(GameState.Exploration);
                        break;
                }
            }
        }
    }
}
