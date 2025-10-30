using System;
using InputSystem;
using Settings;
using TMPro;
using UI;
using UI.Interfaces;
using UnityEngine;

namespace Tutorials
{
    public class TutorialUI : UserInterface
    {
        public override UserInterfaceState State => UserInterfaceState.Tutorial;

        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        
        public static event Action<TutorialInfo> OnTutorialTrigger;
        public static void InvokeOnTutorialTrigger(TutorialInfo info)
        {
            OnTutorialTrigger?.Invoke(info);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            OnTutorialTrigger += ChangeText;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            OnTutorialTrigger -= ChangeText;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Time.timeScale = 0;
            InputManager.Instance.DisableInputs();
            InputManager.Instance.Controlls.UI.Disable();
            
        }
        public override void OnExit()
        {
            base.OnExit();
            Time.timeScale = 1;
            InputManager.Instance.EnableInputs();
            InputManager.Instance.Controlls.UI.Enable();
        }

        private void ChangeText(TutorialInfo info)
        {
            switch (SettingsManager.Instance.Localization.LanguageIndex)
            {
                default:
                    titleText.text = info.titleEnglish;
                    descriptionText.text = info.descriptionEnglish;
                    break;
                case 1: 
                    titleText.text = info.titlePolish;
                    descriptionText.text = info.descriptionPolish;
                    break;
            }
        }
    }
}
