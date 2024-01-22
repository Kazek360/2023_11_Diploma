using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCard : MonoBehaviour
{
    [SerializeField] private AbilityScriptableObject ability;
    [SerializeField] private GameObject abilityNameField;
    [SerializeField] private GameObject abilityImage;
    [SerializeField] private GameObject cooldownCounter;
    [SerializeField] private GameObject cooldownPanel;
    private int _cooldown;
    private void OnEnable()
    {
    }

    // Start is called before the first frame update
    public void SetAbility(AbilityScriptableObject abilityScriptableObject, int cooldown)
    {
        ability = abilityScriptableObject;
        abilityNameField.GetComponent<TextMeshProUGUI>().text = ability.AbilityName;
        abilityImage.GetComponent<Image>().sprite = ability.Sprite;
        if (cooldown > 0)
        {
            cooldownPanel.SetActive(true);
            cooldownCounter.GetComponent<TextMeshProUGUI>().text = ""+cooldown;
            this.GetComponent<Toggle>().interactable = false;
        }
    }

    public void OnAbilityClicked()
    {
        Debug.Log("[UI]: ABILITY '" + ability.AbilityName + "' SELECTED");
    }
}
