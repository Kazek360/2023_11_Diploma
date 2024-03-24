using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/Quests/Quest")]
public class QuestSO : ScriptableObject
{
    public string id;
    
    [Header("General")]
    [SerializeField] public string displayName;
    [SerializeField] public QuestCategory QuestCategory;
    [TextArea][SerializeField] public string description;
    
    [Header("Requirements & steps")]
    [SerializeField] public List<QuestSO> requirements;
    [SerializeField] public List<GameObject> steps;

    private void OnValidate()
    {
        #if UNITY_EDITOR
            id = name;
            UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
}
