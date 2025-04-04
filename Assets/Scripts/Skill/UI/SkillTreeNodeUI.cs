using Skill;
using Skill.Events;
using Skill.Managers;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeNodeUI : MonoBehaviour
{
    [SerializeField] public SkillTreeNodeSO skillTreeNodeInfo;
    [SerializeField] private Image skillDevelopmentIndicator;
    private SkillTreeFacade _parentScript;
    private bool _activated;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        skillDevelopmentIndicator.enabled = false;
        _parentScript = gameObject.GetComponentInParent<SkillTreeFacade>();
        var node = SkillTreeManager.GetNodeByName(skillTreeNodeInfo.name);
        if (node.state == SkillTreeNode.State.Activated)
        {
            ActivateNode();
        }
        gameObject.GetComponent<Toggle>().onValueChanged.AddListener(ValueChanged);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void ActivateNode()
    {
        _activated = true;
        
        skillDevelopmentIndicator.enabled = true;
        foreach (var modifier in skillTreeNodeInfo.appliedModifiers)
        {
            PlayerStats.GetInstance().AddSkillTreeModifier(modifier);
        }
        foreach (var ability in skillTreeNodeInfo.unlockedAbilities)
        {
            UnlockedAbilities.GetInstance().UnlockAbility(ability);
        }
    }
    
    public void ActivateNodeUsingSkillPoints()
    {
        ActivateNode();
        SkillTreeEvents.ActivateNode(skillTreeNodeInfo.name);
    }

    private void ValueChanged(bool val)
    {
        _parentScript.ToggleChosenNode();
    }
}
