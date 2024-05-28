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
        Debug.Log("AAAA");
        ShowAbilities();
    }

    public void OnDisable()
    {
        for (int i = 0; i < AbilitiesScrollContent.transform.childCount; i++) {
            Transform child = AbilitiesScrollContent.transform.GetChild(i);
            Destroy(child.gameObject);
        }
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
