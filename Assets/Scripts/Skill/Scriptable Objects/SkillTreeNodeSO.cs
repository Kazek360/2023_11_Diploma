using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillTreeNode", menuName = "ScriptableObjects/SkillTree/SkillTreeNode")]
public class SkillTreeNodeSO : ScriptableObject
{
    public string id;
    
    [Header("General")]
    [SerializeField] public string displayName;
    [TextArea][SerializeField] public string description;
    
    [Header("Requirements")]
    [SerializeField] public List<SkillTreeNodeSO> requiredNodes;
    
    [Header("Effects")]
    [SerializeField] public List<SkillTreeStatModifier> appliedModifiers;
    
    private void OnValidate()
    {
        #if UNITY_EDITOR
        id = name;
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
}
