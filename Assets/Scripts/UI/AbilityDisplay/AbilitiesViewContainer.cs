using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AbilitiesViewContainer : MonoBehaviour
{
    [SerializeField] private AbilityScriptableObject Ability;
    [SerializeField] private TextMeshProUGUI AbilityNameField;
    [SerializeField] private Image AbilityImage;
    [SerializeField] private TextMeshProUGUI Description;
    [SerializeField] private TextMeshProUGUI CooldownText;
    [SerializeField] private TextMeshProUGUI RangeText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SetAbility(AbilityScriptableObject abilityScriptableObject)
    {
        Ability = abilityScriptableObject;
        Description.text = Ability.GetAbilityActionText();
        CooldownText.text = Ability.GetAbilityCooldownText();
        RangeText.text = Ability.GetAbilityRangeText();
        AbilityNameField.text = Ability.AbilityName;
        AbilityImage.sprite = Ability.Sprite;
    }
}
