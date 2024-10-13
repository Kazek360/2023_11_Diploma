using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/Quests/Quest")]
public class QuestSO : ScriptableObject
{
    [Header("General")]
    [SerializeField] public string displayName;
    [SerializeField] public QuestCategory questCategory;
    [TextArea][SerializeField] public string description;
    
    [Header("Requirements & steps")]
    [SerializeField] public List<QuestSO> requirements;
    [SerializeField] public List<GameObject> steps;

    [Header("Rewards")] 
    [SerializeField] public int experience;
    [SerializeField] public List<int> items;
}
