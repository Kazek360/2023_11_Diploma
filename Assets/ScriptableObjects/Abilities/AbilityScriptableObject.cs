using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

/**
 * A scriptable object for a generic ability, from which other scriptable abilities should inherit
 * Behaviour is created by overriding the CastAbility function
 * Yes, it is terrible
 */
public class AbilityScriptableObject : ScriptableObject
{
    enum AnimType
    {
        Ranged1,
        Ranged2,
        Melee1,
        Melee2,
        Self1,
        Self2
    }

    enum TargetTypes
    {
        Enemy,
        Ally,
        Self,
        Tile
    }
    
    
    
    [Header("Visuals")]
    [SerializeField] private string name;
    [SerializeField] private string description;
    [Tooltip("How will the skill look like on an ability bar")][SerializeField] 
    private Sprite sprite;
    [Tooltip("Determines what animation should be performed by a caster")][SerializeField] 
    private AnimType animType;
    
    [Header("Mechanical")]
    [SerializeField] private int cooldown; //how long will it take for this ability to be available again
    [Tooltip("Measured in hex tiles")] [SerializeField] 
    private int range;
    [Tooltip("What this ability can target")] [SerializeField] 
    private List<TargetTypes> viableTargets;


    // [Header("Health damage")]
    // [SerializeField] private bool dealingHealthDamage;
    // [SerializeField] private int healthDamage;
    //
    // [Header("Psyche damage")]
    // [SerializeField] private bool dealingPsycheDamage;
    // [SerializeField] private int psycheDamage;
    
    public virtual void CastAbility(IHittable target, IHittable caster){}

    
}