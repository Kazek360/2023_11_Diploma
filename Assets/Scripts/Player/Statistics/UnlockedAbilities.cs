using System.Collections;
using System.Collections.Generic;
using Ink.Parsed;
using UnityEngine;

public class UnlockedAbilities
{
    private List<UnlockableAbility> _unlockedAbilities = new List<UnlockableAbility>();

    private static UnlockedAbilities instance;
    
    public UnlockedAbilities GetInstance()
    {
        if (instance is null)
        {
            instance = this;
        }

        return instance;
    }

    public void UnlockAbility(UnlockableAbility ability)
    {
        _unlockedAbilities.Add(ability);
    }

    public List<AbilityScriptableObject> GetAbilities(UnlockableAbility.Recipient recipient)
    {
        List<AbilityScriptableObject> result = new List<AbilityScriptableObject>();
        foreach (UnlockableAbility ability in _unlockedAbilities)
        {
            if (ability.GetRecipient.Equals(recipient))
            {
                result.Add(ability.GetAbilityScriptableObject);
            }
        }

        return result;
    }
}

[System.Serializable]
public class UnlockableAbility
{
    public enum Recipient
    {
        WIT, NESTOR
    }

    [SerializeField] private Recipient _recipient;
    [SerializeField] private AbilityScriptableObject _abilityScriptableObject;

    public Recipient GetRecipient => _recipient;

    public AbilityScriptableObject GetAbilityScriptableObject => _abilityScriptableObject;
}