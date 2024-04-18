using Cutscenes;
using DialogueSystem.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class InputManager : MonoBehaviour, 
        PlayerControlls.IPlayerActions, PlayerControlls.IDialogueActions, PlayerControlls.ICutsceneActions
    {
        private PlayerControlls _controlls;
        
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
            _controlls.Player.SetCallbacks(this);
            _controlls.Dialogue.SetCallbacks(this);
            _controlls.Cutscene.SetCallbacks(this);
            _controlls.Player.Enable();
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
            _controlls.Dialogue.Enable();
        }
        private void OnDialogueFinished(DialogueSO dialogue)
        {
            _controlls.Player.Enable();
            _controlls.Dialogue.Disable();
        }
        public void OnContinueDialogue(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                InputEvents.InputDialogueContinue();
            }
        }

        private void OnCutsceneStarted(CutsceneSO cutscene)
        {
            _controlls.Player.Disable();
            _controlls.Cutscene.Enable();
        }
        private void OnCutsceneFinished()
        {
            _controlls.Player.Enable();
            _controlls.Cutscene.Disable();
        }
        public void OnContinueCutscene(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                InputEvents.InputCutsceneContinue();
            }
        }
    }
}
