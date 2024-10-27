using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityVisualEffects
{
}

public enum AbilityVisualEffectTypes {PROJECTILE};

[Serializable]
public class VisualEffectContainer
{
    [SerializeField] private float durationOrSpeed;
    [SerializeField] private AbilityVisualEffectTypes effectType;
    [SerializeField] private GameObject effectObject;
    
    
    public void ApplyEffect(IHittable caster, IHittable target)
    {
    }
}