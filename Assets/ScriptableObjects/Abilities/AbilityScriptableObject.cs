using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A scriptable object for a generic ability, from which other scriptable abilities should inherit
 * Behaviour is created by overriding the CastAbility function
 * Yes, it is terrible
 */

//[CreateAssetMenu(fileName = "AbilityScriptableObject", menuName = "ScriptableObjects/Abilities/Ability")]
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


    public string AbilityName => abilityName;

    public string AbilityDescription => abilityDescription;

    public Sprite Sprite => sprite;

    public AnimType AnimationType => animType;

    public int Cooldown => cooldown;

    public int Range => range;

    public int NumberOfTargets => numberOfTargets;

    public List<TargetTypes> ViableTargets => viableTargets;

    public virtual void CastAbility(IHittable target, IHittable caster)
    {
        Debug.Log("[COMBAT]: " + caster.GetEntityName() + " USED " + this.abilityName + " ON " + target.GetEntityName());
        //TODO: Play anim here
    }

    public virtual int GetAbilityWeight(IHittable caster)
    {
        return baseAiUseWeight;
    }
    
}