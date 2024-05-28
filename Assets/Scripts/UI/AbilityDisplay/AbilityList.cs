using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AbilityList : MonoBehaviour
{
    [SerializeField] private GameObject AbilitiesScrollContent;
    [SerializeField] private GameObject AbilityDisplay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEnable()
    {
        ShowAbilities();
    }

    public void OnDisable()
    {
        AbilitiesScrollContent.transform.DetachChildren();
    }

    public void ShowAbilities()
    {
        for (int i = 0; i < UnlockedAbilities.GetInstance().GetAbilities(UnlockableAbility.PlayerType.WIT).Count; i++)
        {
            var container = Instantiate(AbilityDisplay);
            AbilityDisplayContainer abilityDisplayContainer = container.gameObject.GetComponent<AbilityDisplayContainer>();
            abilityDisplayContainer.SetAbility(UnlockedAbilities.GetInstance().GetAbilities(UnlockableAbility.PlayerType.WIT)[i]);
            container.transform.SetParent(AbilitiesScrollContent.transform);
        }
    }
}
