using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AbilityCard : MonoBehaviour,   ITooltippable
{
    [FormerlySerializedAs("ability")] [SerializeField] private AbilityScriptableObject Ability;
    [FormerlySerializedAs("abilityNameField")] [SerializeField] private GameObject AbilityNameField;
    [FormerlySerializedAs("abilityImage")] [SerializeField] private GameObject AbilityImage;
    [FormerlySerializedAs("cooldownCounter")] [SerializeField] private GameObject CooldownCounter;
    [FormerlySerializedAs("cooldownPanel")] [SerializeField] private GameObject CooldownPanel;
    [FormerlySerializedAs("actionText")] [SerializeField] private TextMeshProUGUI ActionText;
    [FormerlySerializedAs("cooldownText")] [SerializeField] private TextMeshProUGUI CooldownText;
    [FormerlySerializedAs("rangeText")] [SerializeField] private TextMeshProUGUI RangeText;
    private int _abilityOrderInEntityList;
    private CombatUI _controllerClass;
    private void OnEnable()
    {
    }

    /**
     * Sets attributes for ability card
     *
     * <param name="abilityScriptableObject">scriptable object containing ability information</param>
     * <param name="cooldown">if ability is on cooldown, how many turns are left</param>
     * <param name="orderNumber">what place ability takes in players ability list</param>
     */
    public void SetAbility(AbilityScriptableObject abilityScriptableObject, int cooldown, int orderNumber)
    {
        Ability = abilityScriptableObject;
        // ActionText.text = Ability.GetAbilityActionText();
        CooldownText.text = Ability.GetAbilityCooldownText();
        RangeText.text = Ability.GetAbilityRangeText();
        AbilityNameField.GetComponent<TextMeshProUGUI>().text = Ability.AbilityName;
        AbilityImage.GetComponent<Image>().sprite = Ability.Sprite;
        _abilityOrderInEntityList = orderNumber;
        if (cooldown > 0)
        {
            CooldownPanel.SetActive(true);
            CooldownCounter.GetComponent<TextMeshProUGUI>().text = ""+cooldown;
            this.GetComponent<Toggle>().interactable = false;
        }
    }

    /**
     * Sets 'controller' - CombatUI class within which this object is
     *
     * <param name="newController">Controller for this object</param>
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

    public AbilityScriptableObject GetAbility => Ability;

    public string GetTooltipHeader()
    {
        return Ability.AbilityName;
    }

    public string GetTooltipDesc()
    {
        return Ability.AbilityDescription;
    }
}
