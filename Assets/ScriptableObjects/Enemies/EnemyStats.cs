using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * A scriptable object containing all statistics of an enemy
 */
[CreateAssetMenu(fileName = "EnemyStatsScriptableObject", menuName = "ScriptableObjects/Enemies/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    [SerializeField] private string name;
    [SerializeField] private string description;
    [SerializeField] private int maxHealthPoints;
    [SerializeField] private int maxPsychePoints;
    [SerializeField] private int dexterity;
    [SerializeField] private int strength;
    [SerializeField] private List<DialogueScriptableObject> dialogues;  //TODO: Add abilities


    public string Name => name;
    public int MaxHealthPoints => maxHealthPoints;
    public int MaxPsychePoints => maxPsychePoints;
    public int Dexterity => dexterity;
    public int Strength => strength;
}