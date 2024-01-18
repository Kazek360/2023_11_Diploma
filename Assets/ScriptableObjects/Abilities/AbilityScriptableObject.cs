using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

/**
 * A scriptable object for a generic ability, from which other scriptable abilities should inherit
 * Behaviour is created by overriding the CastAbility function
 * Yes, it is terrible
 */

[CreateAssetMenu(fileName = "MeleeAbilityScriptableObject", menuName = "ScriptableObjects/Abilities/Ability")]
public class AbilityScriptableObject : ScriptableObject
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
        Enemy,
        Ally,
        Self,
        Tile
    }
    
    
    
    [Header("Visuals")]
    [SerializeField] private string name;
    [SerializeField][TextArea] private string description;
    [Tooltip("How will the skill look like on an ability bar")][SerializeField] 
    private Sprite sprite;
    [Tooltip("Determines what animation should be performed by a caster")][SerializeField] 
    private AnimType animType;
    
    [Header("Mechanical")]
    [SerializeField] private int cooldown; //how long will it take for this ability to be available again
    [Tooltip("Measured in hex tiles")] [SerializeField] 
    private int range;
    [SerializeField] private int numberOfTargets;
    [Tooltip("What this ability can target")] [SerializeField] 
    private List<TargetTypes> viableTargets;
    [SerializeField] private int enemyAiUseWeight;


    [Header("Inflicted on the target")]
    [SerializeField] private bool dealingHealthDamage;
    [SerializeField][Min(0)] private int healthDamage;
    [Space(8)]
    // [SerializeField] private bool dealingHealthDot;
    // [SerializeField][Min(0)] private int healthDotDuration;
    // [SerializeField][Min(0)] private int healthDotSeverity;
    [Space(8)]
    [SerializeField] private bool dealingPsycheDamage;
    [SerializeField][Min(0)] private int psycheDamage;
    [Space(8)]
    // [SerializeField] private bool dealingPsycheDot;
    // [SerializeField][Min(0)] private int psycheDotDuration;
    // [SerializeField][Min(0)] private int psycheDotSeverity;
    [Space(8)]
    [SerializeField] private bool healingHealth;
    [SerializeField][Min(0)] private int healthRestored;
    [Space(8)]
    [SerializeField] private bool healingPsyche;
    [SerializeField][Min(0)] private int psycheRestored;
    [Space(8)]
    // [SerializeField] private bool stunsTarget;
    // [SerializeField][Min(0)] private int stunDuration;
    
    
    
    [Space(10)]
    [Header("Inflicted on caster")]
    [SerializeField] private bool dealingHealthDamageSelf;
    [SerializeField][Min(0)] private int healthDamageSelf;
    [Space(8)]
    // [SerializeField] private bool dealingHealthDotSelf;
    // [SerializeField][Min(0)] private int healthDotDurationSelf;
    // [SerializeField][Min(0)] private int healthDotSeveritySelf;
    [Space(8)]
    [SerializeField] private bool dealingPsycheDamageSelf;
    [SerializeField][Min(0)] private int psycheDamageSelf;
    [Space(8)]
    // [SerializeField] private bool dealingPsycheDotSelf;
    // [SerializeField][Min(0)] private int psycheDotDurationSelf;
    // [SerializeField][Min(0)] private int psycheDotSeveritySelf;
    [Space(8)]
    [SerializeField] private bool healingHealthSelf;
    [SerializeField][Min(0)] private int healthRestoredSelf;
    [Space(8)]
    [SerializeField] private bool healingPsycheSelf;
    [SerializeField][Min(0)] private int psycheRestoredSelf;
    // [Space(8)]
    // [SerializeField] private bool stunsTargetSelf;
    // [SerializeField][Min(0)] private int stunDurationSelf;


    public List<TargetTypes> ViableTargets => viableTargets;
    public string Name => name;
    public string Description => description;
    public Sprite Sprite => sprite;
    public AnimType Animtype => animType;
    public int Cooldown => cooldown;
    public int Range => range;
    public int NumberOfTargets => numberOfTargets;
    public int EnemyAiUseWeight => enemyAiUseWeight;
    

    public void CastAbility(IHittable target, IHittable caster)
    {
        if (dealingHealthDamage)
        {
            AbilityEffects.DealHealthDamage(target, healthDamage);
        }
        if (dealingPsycheDamage)
        {
            AbilityEffects.DealPsycheDamage(target, psycheDamage);
        }
        if (healingHealth)
        {
            AbilityEffects.HealHealth(target, healthRestored);
        }
        if (healingPsyche)
        {
            AbilityEffects.HealPsyche(target, psycheRestored);
        }
        
        if (dealingHealthDamageSelf)
        {
            AbilityEffects.DealHealthDamage(caster, healthDamage);
        }
        if (dealingPsycheDamageSelf)
        {
            AbilityEffects.DealPsycheDamage(caster, psycheDamage);
        }
        if (healingHealthSelf)
        {
            AbilityEffects.HealHealth(caster, healthRestored);
        }
        if (healingPsycheSelf)
        {
            AbilityEffects.HealPsyche(caster, psycheRestored);
        }
    }

    
}