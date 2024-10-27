using InputSystem;
using UnityEngine;

namespace UI.Interfaces
{
    public class AbilitiesView : UserInterface
    {
        [SerializeField] private GameObject AbilitiesScrollContent;
        [SerializeField] private GameObject AbilityDisplay;
        public override UserInterfaceState State => UserInterfaceState.Abilities;

        public override void OnEnter()
        {
            base.OnEnter();
            for (int i = 0; i < AbilitiesScrollContent.transform.childCount; i++) {
                Transform child = AbilitiesScrollContent.transform.GetChild(i);
                Destroy(child.gameObject);
            }
            Time.timeScale = 0;
            InputManager.Instance.Controlls.Player.Disable();
            
            ShowAbilities();
        }
        public override void OnExit()
        {
            base.OnExit();
            Time.timeScale = 1;
            InputManager.Instance.Controlls.Player.Enable();
        }

        public void ShowAbilities()
        {
            for (int i = 0; i < UnlockedAbilities.GetInstance().GetAbilities(UnlockableAbility.PlayerType.WIT).Count; i++)
            {
                var container = Instantiate(AbilityDisplay, AbilitiesScrollContent.transform);
                AbilitiesViewContainer abilitiesViewContainer = container.gameObject.GetComponent<AbilitiesViewContainer>();
                abilitiesViewContainer.SetAbility(UnlockedAbilities.GetInstance().GetAbilities(UnlockableAbility.PlayerType.WIT)[i]);
            }
        }
    }
}
