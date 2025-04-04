using System;
using Skill;
using Skill.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillTreeDisplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillPoints;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDesc;
    [SerializeField] private GameObject unlockButton;
    [SerializeField] private GameObject unlockButtonDisabled;

    private SkillTreeNodeSO _selectedSkill;
    private SkillTreeNodeUI _nodeUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _selectedSkill = null;
        skillPoints.text = "Punkty umiejętności: " + PlayerResourcesManager.Instance.SkillPoints;
    }

    private void OnEnable()
    {
        PlayerResourcesEvents.OnSkillPointsValueChanged += SkillButtonAvailability;
    }

    private void OnDisable()
    {
        PlayerResourcesEvents.OnSkillPointsValueChanged -= SkillButtonAvailability;
    }

    public void SetDisplayedSkill(SkillTreeNodeUI nodeUI)
    {
        if (nodeUI is null)
        {
            skillName.text = "";
            skillDesc.text = "";
            _selectedSkill = null;
            _nodeUI = null;
            SkillButtonAvailability(0);
            return;
        }
        //TODO: SKILL COST CHECK
        _nodeUI = nodeUI;
        _selectedSkill = nodeUI.skillTreeNodeInfo;
        skillName.text = nodeUI.skillTreeNodeInfo.displayName;
        skillDesc.text = nodeUI.skillTreeNodeInfo.GetDesc();
        SkillButtonAvailability(0);
    }
    
    private bool CheckIfSkillCanBeUnlocked(int currentSkillPoints, SkillTreeNodeSO skillTreeNodeInfo)
    {
        var node = SkillTreeManager.GetNodeByName(skillTreeNodeInfo.name);
        
        if (node.state == SkillTreeNode.State.CannotBeActivated || node.state == SkillTreeNode.State.Activated)
        {
            return false;
        }

        if (currentSkillPoints >= skillTreeNodeInfo.skillPointsCost)
        {
            return true;
        }
        return false;
    }

    /**
     * Switches buttons on and off depending on whether they are supposed to be unlocked or not
     *
     * @param n - not important, inserted to fit with Oskar's event
     */
    public void SkillButtonAvailability(int n)
    {
        if (_selectedSkill is null)
        {
            return;
        }
        if (CheckIfSkillCanBeUnlocked(PlayerResourcesManager.Instance.SkillPoints, _selectedSkill))
        {
            unlockButtonDisabled.SetActive(false);
            unlockButton.SetActive(true);
        }
        else
        {
            unlockButtonDisabled.SetActive(true);
            unlockButton.SetActive(false);
        }
    }

    public void UnlockSkill()
    {
        _nodeUI.ActivateNodeUsingSkillPoints();
    }
}
