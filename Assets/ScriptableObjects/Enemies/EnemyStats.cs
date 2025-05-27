using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

/**
 * A scriptable object containing all statistics of an enemy
 */
[CreateAssetMenu(fileName = "EnemyStatsScriptableObject", menuName = "ScriptableObjects/Enemies/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    [FormerlySerializedAs("name")] [SerializeField] private string enemyName;
    [SerializeField][TextArea] private string description;
    [SerializeField] private int maxHealthPoints;
    [SerializeField] private int maxPsychePoints;
    [SerializeField] private int agility;
    [SerializeField] private int strength;
    [SerializeField] private int luck;
    [SerializeField] private int imagination;
    [SerializeField] private int preferredRange;
    [SerializeField] private List<AbilityScriptableObjectReworked> abilities;
    [SerializeField] private Sprite enemyImage;


    public string EnemyName => enemyName;

    public string Description => description;

    public int MaxHealthPoints => maxHealthPoints;
    public int MaxPsychePoints => maxPsychePoints;
    public int Agility => agility;
    public int Strength => strength;
    public int Luck => luck;
    public int Imagination => imagination;

    public int PreferredRange => preferredRange;

    public List<AbilityScriptableObjectReworked> Abilities => abilities;

    public Sprite EnemyImage => enemyImage;
}