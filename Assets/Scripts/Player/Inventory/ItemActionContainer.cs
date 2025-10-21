using System;
using System.Collections;
using System.Collections.Generic;
using StorySystems.Dialogues;
using UnityEngine;

[Serializable]
public class ItemActionContainer
{
    public enum ActionType
    {
        ADD_ABILITY, REMOVE_ABILITY, TRIGGER_TAG, FIRE_DIALOGUE
    }

    [SerializeField] private ActionType actionType;
    [Tooltip("ID of added ability")][SerializeField] private int idOfEffect;
    [Tooltip("Value of triggered TAG")][SerializeField] private String valueOfEffect;

    public void Trigger()
    {
        switch (actionType)
        {
            case ActionType.ADD_ABILITY:
            {
                UnlockableAbility ability = new UnlockableAbility();
                ability.AbilitySO = GameConfig.abilityLibrary.GetAbility(idOfEffect);
                ability.Recipient = UnlockableAbility.PlayerType.WIT;
                UnlockedAbilities.GetInstance().UnlockAbility(ability);
                break;
            }
            case ActionType.REMOVE_ABILITY:
            {
                UnlockableAbility ability = new UnlockableAbility();
                ability.AbilitySO = GameConfig.abilityLibrary.GetAbility(idOfEffect);
                ability.Recipient = UnlockableAbility.PlayerType.WIT;
                UnlockedAbilities.GetInstance().RemoveAbility(ability);
                break;
            }
            case ActionType.TRIGGER_TAG:
            {
                break;
            }
            case ActionType.FIRE_DIALOGUE:
            {
                Debug.Log("DIALOG ODPALONY: " + valueOfEffect);
                DialogueEvent.TriggerDialogue(valueOfEffect);
                break;
            }
            default:
            {
                break;
            }
        }
    }
    
}

[Serializable]
public class StatModifierContainer
{
    [SerializeField] private int amount;
    [SerializeField] private EffectedStat effectedStat;

}

[Serializable]
public class ItemEffectContainer
{
    [SerializeField] private int amount;
    [SerializeField] private EffectedVariable effectedStat;
    
    public void ApplyEffect()
    {
        int fullValue = amount;

        if (effectedStat == EffectedVariable.HEALTH)
        {
            PlayerStats.GetInstance().ModifyWitHealth(fullValue);
        } else if (effectedStat == EffectedVariable.PSYCHE)
        {
            //intendedTarget.ModifyPsyche(fullValue);
        }
    }
}