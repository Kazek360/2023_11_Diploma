using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityVisualEffects: MonoBehaviour
{
    public static void ApplyEffect(IHittable caster, IHittable target, float durationOrSpeed, AbilityVisualEffectTypes effectType, GameObject effectObject)
    {
        if (effectType == AbilityVisualEffectTypes.PROJECTILE)
        {
            var projectile = Instantiate(effectObject);
            projectile.transform.position = caster.GetObject().transform.position;
            projectile.GetComponent<ProjectileScript>().SetTarget(target.GetObject());
        }
        if (effectType == AbilityVisualEffectTypes.BANG)
        {
            var bang = Instantiate(effectObject);
            bang.transform.position = target.GetObject().transform.position;
        }
    }
}

public enum AbilityVisualEffectTypes {PROJECTILE, BANG};

[Serializable]
public class VisualEffectContainer
{
    [SerializeField] private float durationOrSpeed;
    [SerializeField] private AbilityVisualEffectTypes effectType;
    [SerializeField] private GameObject effectObject;

    public float DurationOrSpeed => durationOrSpeed;

    public AbilityVisualEffectTypes EffectType => effectType;

    public GameObject EffectObject => effectObject;
}