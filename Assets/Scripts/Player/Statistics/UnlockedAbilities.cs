using System.Collections;
using System.Collections.Generic;
using Ink.Parsed;
using UnityEngine;
using UnityEngine.Serialization;

public class UnlockedAbilities
{
    private List<UnlockableAbility> _unlockedAbilities;

    private static UnlockedAbilities instance;
    
    public static UnlockedAbilities GetInstance()
    {
        if (instance is null)
        {
            instance = new UnlockedAbilities();
        }

        return instance;
    }

    private UnlockedAbilities()
    {
        _unlockedAbilities = new List<UnlockableAbility>();
    }

    public void UnlockAbility(UnlockableAbility ability)
    {
        _unlockedAbilities.Add(ability);
    }

    public List<AbilityScriptableObject> GetAbilities(UnlockableAbility.PlayerType playerType)
    {
        List<AbilityScriptableObject> result = new List<AbilityScriptableObject>();
        foreach (UnlockableAbility ability in _unlockedAbilities)
        {
            if (ability.Recipient.Equals(playerType))
            {
                result.Add(ability.AbilitySO);
            }
        }

        return result;
    }

    private void Save()
    {
        //TODO: OSKAR I MECHANIZM ZAPISU
    }

    private void Load()
    {
        //TODO: OSKAR I MECHANIZM ZAPISU
    }
}

[System.Serializable]
public class UnlockableAbility
{
    public enum PlayerType
    {
        WIT, NESTOR
    }

    [FormerlySerializedAs("_recipient")] [SerializeField] private PlayerType recipient;
    [SerializeField] private AbilityScriptableObject _abilityScriptableObject;

    public PlayerType Recipient
    {
        get => recipient;
        set => recipient = value;
    }

    public AbilityScriptableObject AbilitySO
    {
        get => _abilityScriptableObject;
        set => _abilityScriptableObject = value;
    }
}