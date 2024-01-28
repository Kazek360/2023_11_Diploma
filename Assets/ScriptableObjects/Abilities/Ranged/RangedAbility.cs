using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeAbilityScriptableObject", menuName = "ScriptableObjects/Abilities/RangedAbility")]
public class RangedAbility : AbilityScriptableObject
{
    [SerializeField] private GameObject projectilePrefab;
    
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
            //AbilityEffects.DealHealthDamage(target, healthDamage + caster.GetStrength());
            AbilityEffects.DealHealthDamage(target, healthDamage);
        }
        if (dealingPsycheDamage)
        {
            //AbilityEffects.DealPsycheDamage(target, psycheDamage + caster.GetImagination());
        }

        var projectile = Instantiate(projectilePrefab);
        projectile.transform.position = caster.GetObject().transform.position;
        projectile.GetComponent<ProjectileScript>().SetTarget(target.GetObject());
        
    }

    public override string GetAbilityActionText()
    {
        return "DAMAGE: " + healthDamage;
    }
}