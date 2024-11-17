using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemActionContainer
{
    public enum ActionType
    {
        ADD_ABILITY, REMOVE_ABILITY, TRIGGER_TAG
    }

    [SerializeField] private ActionType actionType;
    [Tooltip("ID of added ability")][SerializeField] private int idOfEffect;
    [Tooltip("Value of triggered TAG")][SerializeField] private int valueOfEffect;
    
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