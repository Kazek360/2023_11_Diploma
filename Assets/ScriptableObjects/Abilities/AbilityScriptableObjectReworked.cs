using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A scriptable object for a generic ability, from which other scriptable abilities should inherit
 * Behaviour is created by overriding the CastAbility function
 * Yes, it is terrible
 */

[CreateAssetMenu(fileName = "AbilityScriptableObjectReworked", menuName = "ScriptableObjects/Abilities/AbilityRework")]
public class AbilityScriptableObjectReworked : ScriptableObject
{
    public enum AnimType
    {
        Ranged1,
        Ranged2,
        Melee1,
        Melee2,
        Self1,
        Self2
    }

    public enum TargetTypes
    {
        EnemyMaterial,
        AllyMaterial,
        EnemyImmaterial,
        AllyImmaterial
    }
    
    
    
    [Header("Visuals")]
    [SerializeField] private string abilityName;
    [SerializeField][TextArea] private string abilityDescription;
    [Tooltip("How will the skill look like on an ability bar")][SerializeField] 
    private Sprite sprite;
    [Tooltip("Determines what animation should be performed by a caster")][SerializeField] 
    private AnimType animType;
    
    [Header("Mechanical")]
    [SerializeField] private int cooldown; //how long will it take for this ability to be available again(in turns)
    [Tooltip("Measured in hex tiles")] [SerializeField] 
    private int range;
    [SerializeField] private int numberOfTargets;
    [Tooltip("What this ability can target")] [SerializeField] 
    private List<TargetTypes> viableTargets;
    [SerializeField] private int baseAiUseWeight;
    [Tooltip("What does the ability do")] [SerializeField] 
    private List<EffectContainer> effects;
    [Tooltip("What does the ability do over time")] [SerializeField] 
    private List<EffectOverTimeContainer> effectsOverTime;
    [Tooltip("What visual effects does the ability invoke")] [SerializeField] 
    private List<VisualEffectContainer> visualEffects;


    public string AbilityName => abilityName;

    public string AbilityDescription => abilityDescription;

    public Sprite Sprite => sprite;

    public AnimType AnimationType => animType;

    public int Cooldown => cooldown;

    public int Range => range;

    public int NumberOfTargets => numberOfTargets;

    public List<TargetTypes> ViableTargets => viableTargets;

    /**
     * A function defining behaviour of abilities
     *
     * Needs to extend from AbilityScriptableObject equivalent in order to work
     */
    public virtual void CastAbility(IHittable target, IHittable caster)
    {
        Debug.Log("[COMBAT]: " + caster.GetEntityName() + " USED " + this.abilityName + " ON " + target.GetEntityName());

        foreach (var effect in effects)
        {
            effect.ApplyEffect(caster, target);
        }

        foreach (var effectOverTime in effectsOverTime)
        {
            //TODO: IMPLEMENT OVER TIME EFFECTS
        }
        
        foreach (var visualEffect in visualEffects)
        {
            AbilityVisualEffects.ApplyEffect(caster, target, visualEffect.DurationOrSpeed, visualEffect.EffectType, visualEffect.EffectObject);
        }
        //TODO: Play anim here
    }

    /**
     * Returns how likely AI is to use this ability
     * If need arises, additional modifications may be done in inheriting classes
     */
    public virtual int GetAbilityWeight(IHittable caster)
    {
        return baseAiUseWeight;
    }

    public virtual string GetAbilityActionText()
    {
        return "";
    }
    
    public string GetAbilityCooldownText()
    {
        return "ODNOWIENIE: " + (cooldown);
    }
    
    public string GetAbilityRangeText()
    {
        return "ZASIĘG: " + range;
    }

}