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
    [SerializeField] private TextMeshProUGUI actionText;
    [SerializeField] private TextMeshProUGUI cooldownText;
    [SerializeField] private TextMeshProUGUI rangeText;
    private int _abilityOrderInEntityList;
    private CombatUI _controllerClass;
    private void OnEnable()
    {
    }

    /**
     * Sets attributes for ability card
     *
     * abilityScriptableObject - scriptable object containing ability information
     * cooldown - if ability is on cooldown, how many turns are left
     * orderNumber - what place ability takes in players ability list
     */
    public void SetAbility(AbilityScriptableObject abilityScriptableObject, int cooldown, int orderNumber)
    {
        ability = abilityScriptableObject;
        actionText.text = ability.GetAbilityActionText();
        cooldownText.text = ability.GetAbilityCooldownText();
        rangeText.text = ability.GetAbilityRangeText();
        abilityNameField.GetComponent<TextMeshProUGUI>().text = ability.AbilityName;
        abilityImage.GetComponent<Image>().sprite = ability.Sprite;
        _abilityOrderInEntityList = orderNumber;
        if (cooldown > 0)
        {
            cooldownPanel.SetActive(true);
            cooldownCounter.GetComponent<TextMeshProUGUI>().text = ""+cooldown;
            this.GetComponent<Toggle>().interactable = false;
        }
    }

    /**
     * Sets 'controller' - CombatUI class within which this object is
     */
    public void SetController(CombatUI newController)
    {
        _controllerClass = newController;
    }

    /**
     * If turned on, notifies controller about it
     */
    public void OnClicked()
    {
        _controllerClass.OnNewAbilitySelected();
    }
    

    public int AbilityOrderInEntityList => _abilityOrderInEntityList;

    public AbilityScriptableObject Ability => ability;
}
