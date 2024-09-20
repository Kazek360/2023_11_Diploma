using InputSystem;
using UnityEngine;

public class AbilityDisplaySwitch : MonoBehaviour
{

    [SerializeField] private GameObject abilityScreen;

    private void OnEnable()
    {
        InputEvents.OnAbilityViewToggle += ToggleAbilityView;
        InputEvents.OnInterfaceClosed += CloseAbilityView;
    }

    private void OnDisable()
    {
        InputEvents.OnAbilityViewToggle -= ToggleAbilityView;
        InputEvents.OnInterfaceClosed -= CloseAbilityView;
    }

    private void ToggleAbilityView()
    {
        if (abilityScreen.activeSelf)
        {
            abilityScreen.SetActive(false);
        }
        else
        {
            abilityScreen.SetActive(true);
        }
    }
    
    private void CloseAbilityView()
    {
        abilityScreen.SetActive(false);
    }
}
