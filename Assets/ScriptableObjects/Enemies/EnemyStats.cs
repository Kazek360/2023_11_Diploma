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
    [SerializeField] private string description;
    [SerializeField] private int maxHealthPoints;
    [SerializeField] private int maxPsychePoints;
    [SerializeField] private int dexterity;
    [SerializeField] private int strength;
    [SerializeField] private List<AbilityScriptableObject> abilities;
    [SerializeField] private Sprite enemyImage;


    public string EnemyName => enemyName;
    public int MaxHealthPoints => maxHealthPoints;
    public int MaxPsychePoints => maxPsychePoints;
    public int Dexterity => dexterity;
    public int Strength => strength;

    public List<AbilityScriptableObject> Abilities => abilities;

    public Sprite EnemyImage => enemyImage;
}