using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeAbilityScriptableObject", menuName = "ScriptableObjects/Abilities/RangedAbility")]
public class RangedAbility : AbilityScriptableObject
{
    [SerializeField] private GameObject projectile;     //TODO: THAT'S FOR CONTENT TEAM TO SET UP
    [SerializeField] private int projectileSpeed;
    
    [SerializeField] private bool dealingHealthDamage;
    [SerializeField][Min(0)] private int healthDamage;
    [Space(8)]
    [SerializeField] private bool dealingPsycheDamage;
    [SerializeField][Min(0)] private int psycheDamage;

    public override void CastAbility(IHittable target, IHittable caster)
    {
        base.CastAbility(target, caster);
        if (dealingHealthDamage)
        {
            AbilityEffects.DealHealthDamage(target, healthDamage + caster.GetStrength());
        }
        if (dealingPsycheDamage)
        {
            AbilityEffects.DealPsycheDamage(target, psycheDamage + caster.GetImagination());
        }
    }
}