using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MeleeAbilityScriptableObject", menuName = "ScriptableObjects/Abilities/BasicMeleeAbility")]
public class BasicMelee : AbilityScriptableObject
{
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
