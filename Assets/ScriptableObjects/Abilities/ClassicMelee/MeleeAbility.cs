using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeAbilityScriptableObject", menuName = "ScriptableObjects/Abilities/MeleeAbility")]
public class MeleeAbility : AbilityScriptableObject
{
    [Header("Generic Damage")] 
    [Min(0)] [SerializeField] private int healthDamage;
    [Min(0)] [SerializeField] private int psycheDamage;
    
    //TODO
    //[Header("Timed Modifiers")] 
    
    
    public override void CastAbility(IHittable target, IHittable caster)
    {
        AbilityEffects.DealHealthDamage(target, healthDamage);
        AbilityEffects.DealPsycheDamage(target, psycheDamage);
    }
}
