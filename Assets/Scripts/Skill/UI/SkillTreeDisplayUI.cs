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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        skillPoints.text = "Punkty umiejętności: " + PlayerResourcesManager.Instance.SkillPoints;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDisplayedSkill(SkillTreeNodeSO skillTreeNodeSo)
    {
        if (skillTreeNodeSo is null)
        {
            skillName.text = "";
            skillDesc.text = "";
            unlockButtonDisabled.SetActive(true);
            unlockButton.SetActive(false);
            return;
        }
        //TODO: SKILL COST CHECK
        skillName.text = skillTreeNodeSo.displayName;
        skillDesc.text = skillTreeNodeSo.description;
        if (CheckIfSkillCanBeUnlocked(PlayerResourcesManager.Instance.SkillPoints, skillTreeNodeSo))
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
}
