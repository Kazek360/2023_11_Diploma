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
        PlayerControlls.IPlayerActions, PlayerControlls.IStoryActions,
        PlayerControlls.IUIActions, PlayerControlls.IPauseMenuActions
    {
        private PlayerControlls _controlls;
        public PlayerControlls Controlls => _controlls;

        private bool _uiOpened;
        private string _uiName;
        
        private void OnDisable()
        {
            _controlls.Player.RemoveCallbacks(this);
            _controlls.Story.RemoveCallbacks(this);
            _controlls.UI.RemoveCallbacks(this);
            _controlls.PauseMenu.RemoveCallbacks(this);
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
            _uiOpened = false;
            _uiName = "";
            
            _controlls.Player.SetCallbacks(this);
            _controlls.Story.SetCallbacks(this);
            _controlls.UI.SetCallbacks(this);
            _controlls.PauseMenu.SetCallbacks(this);
            
            _controlls.Player.Enable();
            _controlls.UI.Enable();
            _controlls.PauseMenu.Enable();

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

        private void PrepareInterfaceValues(string uiName)
        {
            if (_uiOpened)
            {
                _uiName = ""; 
                _controlls.Player.Enable();
                _controlls.PauseMenu.Enable();
            }
            else
            {
                _uiName = uiName;
                _controlls.Player.Disable();
                if (!uiName.Equals("QuestLog"))
                {
                    _controlls.PauseMenu.Disable();
                }
            }
            _uiOpened = !_uiOpened;
        }
        
        public void OnToggleQuestLog(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (!_uiName.Equals("QuestLog") && !_uiName.Equals(""))
                {
                    return;
                }
                PrepareInterfaceValues("QuestLog");
                if (UserInterfaceManager.Instance.CurrentStateType == GameState.Exploration)
                {
                    UserInterfaceManager.Instance.ChangeState(GameState.Quests);    
                }
                else if (UserInterfaceManager.Instance.CurrentStateType == GameState.Quests)
                {
                    UserInterfaceManager.Instance.ChangeState(GameState.Exploration);  
                }
            }
        }
        
        public void OnToggleSkillTree(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (!_uiName.Equals("SkillTree") && !_uiName.Equals(""))
                {
                    return;
                }
                PrepareInterfaceValues("SkillTree");
                InputEvents.ToggleSkillTree();
            }
        }

        public void OnToggleInventory(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (!_uiName.Equals("Inventory") && !_uiName.Equals(""))
                {
                    return;
                }
                PrepareInterfaceValues("Inventory");
                InputEvents.ToggleInventory();
            }
        }

        public void OnToggleAbilityView(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (!_uiName.Equals("AbilityView") && !_uiName.Equals(""))
                {
                    return;
                }
                PrepareInterfaceValues("AbilityView");
                InputEvents.ToggleAbilityView();
            }
        }

        public void OnCloseUI(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _controlls.Player.Enable();
                _controlls.PauseMenu.Enable();
                _uiOpened = false;
                _uiName = ""; 
                InputEvents.CloseInterface();
            }
        }

        public void OnTogglePauseMenu(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                switch (UserInterfaceManager.Instance.CurrentStateType)
                {
                    case GameState.Exploration or GameState.LoadGame or GameState.SaveGame:
                        UserInterfaceManager.Instance.ChangeState(GameState.PauseMenu);
                        break;
                    case GameState.PauseMenu or GameState.Quests:
                        UserInterfaceManager.Instance.ChangeState(GameState.Exploration);
                        break;
                }
            }
        }
    }
}
