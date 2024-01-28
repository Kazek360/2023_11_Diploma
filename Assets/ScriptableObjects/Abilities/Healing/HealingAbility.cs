using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeAbilityScriptableObject", menuName = "ScriptableObjects/Abilities/HealingAbility")]
public class HealingAbility : AbilityScriptableObject
{
    [SerializeField] private bool healingHealth;
    [SerializeField][Min(0)] private int healthRestored;
    [Space(8)]
    [SerializeField] private bool healingPsyche;
    [SerializeField][Min(0)] private int psycheRestored;
    public override void CastAbility(IHittable target, IHittable caster)
    {
        base.CastAbility(target, caster);
        if (healingHealth)
        {
            AbilityEffects.HealHealth(target, healthRestored);
        }
        if (healingPsyche)
        {
            AbilityEffects.HealPsyche(target, psycheRestored);
        }
    }
    
    public override string GetAbilityActionText()
    {
        return "HEALING: " + healthRestored;
    }
}
