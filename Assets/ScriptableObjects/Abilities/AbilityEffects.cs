using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityEffects : MonoBehaviour
{
    /**
     * What I'm about to do is not just stupid - it is a monument to all my sins as a programmer
     * It is a manifesto of desperation and hubris
     * It is a clear sign that I should have chosen Extended Polish instead of Extended Math and that giving
     * me access to a working computer was the greatest mistake of our universe
     *
     * But it is also the only way to do this without using at least 2000 classes that I could think of
     * May God forgive me
     *
     * Ok, so what you have here is a list of all modifiers that an ability can apply to any entity
     * They will be used to create every ability
     * And yes, they all have to be here because otherwise it would devolve into either
     * writing every ability from scratch or creating some convoluted JSON-parsing system that
     * would plague my dreams
     */

    public static void DealHealthDamage(IHittable target, int damage)
    {
        target.ModifyHealth(-damage);
    }

    public static void HealHealth(IHittable target, int amount)
    {
        target.ModifyHealth(Math.Min(amount, target.GetMaxHealth() - target.GetCurrentHealth()));
    }
    
    public static void DealPsycheDamage(IHittable target, int damage)
    {
        target.ModifyPsyche(-damage);
    }
    
    public static void HealPsyche(IHittable target, int amount)
    {
        target.ModifyPsyche(Math.Min(amount, target.GetMaxPsyche() - target.GetCurrentPsyche()));
    }

    public static void PsycheDamageOverTime(IHittable target, int damage, int duration)
    {
        
    }

    public static void AAAAAAA()
    {
        Debug.Log("AAAAAAA");
    }
}

public enum EffectedStat{AGILITY, HEALTH, IMAGINATION, LUCK, STRENGTH}
public enum EffectedVariable{HEALTH, PSYCHE}

[Serializable]
public class EffectModifier
{
    [SerializeField] private float scaling;
    [SerializeField] private EffectedStat modifyingStat;

    public int GetModifierValue(IHittable caster)
    {
        switch (modifyingStat)
        {
            case EffectedStat.LUCK:
                return (int)(caster.GetLuck() * scaling);
            case EffectedStat.HEALTH:
                return (int)(caster.GetMaxHealth() * scaling);
            case EffectedStat.AGILITY:
                return (int)(caster.GetAgility() * scaling);
            case EffectedStat.STRENGTH:
                return (int)(caster.GetStrength() * scaling);
            case EffectedStat.IMAGINATION:
                return (int)(caster.GetImagination() * scaling);
            default:
                return 0;
        }
    }

}


[Serializable]
public class EffectContainer
{
    [SerializeField] private int amount;
    [SerializeField] private EffectedVariable effectedStat;
    [SerializeField] private bool effectCaster;
    [SerializeField] private List<EffectModifier> effectModifiers;
    
    public void ApplyEffect(IHittable caster, IHittable target)
    {
        int fullValue = amount;
        foreach (var modifier in effectModifiers)
        {
            fullValue += modifier.GetModifierValue(caster);
        }

        IHittable intendedTarget = target;
        if (effectCaster)
        {
            intendedTarget = caster;
        }

        if (effectedStat == EffectedVariable.HEALTH)
        {
            intendedTarget.ModifyHealth(fullValue);
        } else if (effectedStat == EffectedVariable.PSYCHE)
        {
            intendedTarget.ModifyPsyche(fullValue);
        }
    }
}


[Serializable]
public class EffectOverTimeContainer
{
    [SerializeField] private int amount;
    [SerializeField] private int duration;
    [SerializeField] private bool effectCaster;
    [SerializeField] private EffectedVariable effectedStat;
    
    public void ApplyEffect(IHittable caster, IHittable target, int damage, int duration)
    {
    }
}