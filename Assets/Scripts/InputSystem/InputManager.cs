using Cutscenes;
using DialogueSystem.Events;
using DialogueSystem.ScriptableObjects;
using Shared;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class InputManager : Singleton<InputManager>, 
        PlayerControlls.IPlayerActions, PlayerControlls.IDialogueActions,
        PlayerControlls.IUIActions, PlayerControlls.IPauseMenuActions
    {
        private PlayerControlls _controlls;
        public PlayerControlls Controlls => _controlls;

        private bool _uiOpened;
        private string _uiName;
        
        private void OnEnable()
        {
            DialogueEvents.OnDialogueStarted += OnDialogueStarted;
            DialogueEvents.OnDialogueFinished += OnDialogueFinished;
            CutsceneEvents.OnCutsceneStarted += OnCutsceneStarted;
            CutsceneEvents.OnCutsceneFinished += OnCutsceneFinished;
        }
        
        private void OnDisable()
        {
            DialogueEvents.OnDialogueStarted -= OnDialogueStarted;
            DialogueEvents.OnDialogueFinished -= OnDialogueFinished;
            CutsceneEvents.OnCutsceneStarted -= OnCutsceneStarted;
            CutsceneEvents.OnCutsceneFinished -= OnCutsceneFinished;
            
            _controlls.Player.RemoveCallbacks(this);
            _controlls.Dialogue.RemoveCallbacks(this);
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
            _controlls.Dialogue.SetCallbacks(this);
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
                InputEvents.InputInteraction();
            }
        }
        
        private void OnDialogueStarted(DialogueSO dialogue)
        {
            _controlls.Player.Disable();
            _controlls.UI.Disable();
            _controlls.PauseMenu.Disable();
            _controlls.Dialogue.Enable();
        }
        private void OnDialogueFinished(DialogueSO dialogue)
        {
            _controlls.Player.Enable();
            _controlls.UI.Enable();
            _controlls.PauseMenu.Enable();
            _controlls.Dialogue.Disable();
        }
        private void OnCutsceneStarted(CutsceneSO cutscene)
        {
            _controlls.Player.Disable();
            _controlls.UI.Disable();
            _controlls.PauseMenu.Disable();
        }
        private void OnCutsceneFinished()
        {
            _controlls.Player.Enable();
            _controlls.UI.Enable();
            _controlls.PauseMenu.Enable();
        }
        
        public void OnContinueDialogue(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                InputEvents.InputDialogueContinue();
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
                _controlls.PauseMenu.Disable();
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
                InputEvents.ToggleQuestLog();
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
                    case GameState.PauseMenu:
                        UserInterfaceManager.Instance.ChangeState(GameState.Exploration);
                        break;
                }
            }
        }
    }
}
