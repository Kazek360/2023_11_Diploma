using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillTreeNode", menuName = "ScriptableObjects/SkillTree/SkillTreeNode")]
public class SkillTreeNodeSO : ScriptableObject
{ 
    [Header("General")]
    [SerializeField] public string displayName;
    [TextArea][SerializeField] public string description;
    
    [Header("Requirements")]
    [SerializeField] public List<SkillTreeNodeSO> requiredNodes;
    [SerializeField] public int skillPointsCost;
    
    [Header("Effects")]
    [SerializeField] public List<SkillTreeStatModifier> appliedModifiers;
    [SerializeField] public List<UnlockableAbility> unlockedAbilities;
}
