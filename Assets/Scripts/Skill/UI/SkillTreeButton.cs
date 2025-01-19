using System.Text;
using Skill;
using Skill.Events;
using Skill.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeButton : MonoBehaviour, ITooltippable
{
    [SerializeField] public Button button;
    [SerializeField] public SkillTreeNodeSO skillTreeNodeInfo;

    public void OnEnable()
    {
        PlayerResourcesEvents.OnSkillPointsValueChanged += CheckIfNodeCanBeEnabled;
        if (PlayerResourcesManager.Instance is not null)
        {
            CheckIfNodeCanBeEnabled(PlayerResourcesManager.Instance.SkillPoints);
        }
    }
    
    public void OnDisable()
    {
        PlayerResourcesEvents.OnSkillPointsValueChanged -= CheckIfNodeCanBeEnabled;
    }

    public void Start()
    {
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = skillTreeNodeInfo.displayName;
        if (PlayerResourcesManager.Instance.SkillPoints == 0)
        {
            button.enabled = false;
            button.image.color = Color.gray;
        }
        var node = SkillTreeManager.GetNodeByName(skillTreeNodeInfo.name);
        if (node.state == SkillTreeNode.State.Activated)
        {
            ActivateNode();
        }
    }

    private void CheckIfNodeCanBeEnabled(int value)
    {
        var node = SkillTreeManager.GetNodeByName(skillTreeNodeInfo.name);
        if (node.state == SkillTreeNode.State.Activated || node.state == SkillTreeNode.State.CannotBeActivated)
        {
            return;
        }
        
        switch (value)
        {
            case > 0:
                button.enabled = true;
                button.image.color = Color.white;
                break;
            case 0:
                button.enabled = false;
                button.image.color = Color.gray;
                break;
        }
    }
    private void ActivateNode()
    {
        button.enabled = false;
        button.image.color = Color.cyan;
        foreach (var modifier in skillTreeNodeInfo.appliedModifiers)
        {
            PlayerStats.GetInstance().AddSkillTreeModifier(modifier);
        }
        foreach (var ability in skillTreeNodeInfo.unlockedAbilities)
        {
            UnlockedAbilities.GetInstance().UnlockAbility(ability);
        }
    }
    
    public void ActivateNodeUsingSkillPoint()
    {
        ActivateNode();
        SkillTreeEvents.ActivateNode(skillTreeNodeInfo.name);
    }
    
    public string GetTooltipHeader()
    {
        return skillTreeNodeInfo.displayName;
    }

    public string GetTooltipDesc()
    {
        StringBuilder str = new StringBuilder();
        str.Append(skillTreeNodeInfo.description);
        if (skillTreeNodeInfo.appliedModifiers.Count > 0)
        {
            str.Append("\nEfekty:");
            foreach (var modifier in skillTreeNodeInfo.appliedModifiers)
            {
                str.Append("\n\t" + modifier.StatString());
            }
        }

        if (skillTreeNodeInfo.unlockedAbilities.Count > 0)
        {
            str.Append("\nZdolności:");
            foreach (var ability in skillTreeNodeInfo.unlockedAbilities)
            {
                str.Append("\n\t" + ability.AbilitySO.AbilityName);
            }
        }
        
        return str.ToString();
    }
}
