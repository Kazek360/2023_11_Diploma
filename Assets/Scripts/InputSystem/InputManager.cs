using Cutscenes;
using DialogueSystem.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class InputManager : MonoBehaviour, 
        PlayerControlls.IPlayerActions, PlayerControlls.IDialogueActions, PlayerControlls.ICutsceneActions,
        PlayerControlls.IUIActions
    {
        private PlayerControlls _controlls;
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
        }
        
        private void Awake()
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
            _controlls.Cutscene.SetCallbacks(this);
            _controlls.UI.SetCallbacks(this);
            
            _controlls.Player.Enable();
            _controlls.UI.Enable();
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
            _controlls.Dialogue.Enable();
        }
        private void OnDialogueFinished(DialogueSO dialogue)
        {
            _controlls.Player.Enable();
            _controlls.UI.Enable();
            _controlls.Dialogue.Disable();
        }
        private void OnCutsceneStarted(CutsceneSO cutscene)
        {
            _controlls.Player.Disable();
            _controlls.UI.Disable();
            _controlls.Cutscene.Enable();
        }
        private void OnCutsceneFinished()
        {
            _controlls.Player.Enable();
            _controlls.UI.Enable();
            _controlls.Cutscene.Disable();
        }
        
        public void OnContinueDialogue(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                InputEvents.InputDialogueContinue();
            }
        }
        public void OnContinueCutscene(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                InputEvents.InputCutsceneContinue();
            }
        }

        private void PrepareInterfaceValues(string uiName)
        {
            if (_uiOpened)
            {
                _uiName = ""; 
                _controlls.Player.Enable();
            }
            else
            {
                _uiName = uiName;
                _controlls.Player.Disable();
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

        public void OnCloseUI(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _controlls.Player.Enable();
                _uiOpened = false;
                _uiName = ""; 
                InputEvents.CloseInterface();
            }
        }
    }
}
