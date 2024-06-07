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
        PlayerResourcesEvents.OnSkillPointsChange += CheckIfNodeCanBeEnabled;
        if (PlayerResourcesManager.Instance is not null)
        {
            CheckIfNodeCanBeEnabled(PlayerResourcesManager.Instance.SkillPoints);
        }
    }
    
    public void OnDisable()
    {
        PlayerResourcesEvents.OnSkillPointsChange -= CheckIfNodeCanBeEnabled;
    }

    public void Start()
    {
        if (PlayerResourcesManager.Instance.SkillPoints == 0)
        {
            button.enabled = false;
            button.image.color = Color.gray;
        }
    }

    private void CheckIfNodeCanBeEnabled(int value)
    {
        var node = SkillTreeManager.GetNodeById(skillTreeNodeInfo.id);
        if (node.state == SkillTreeNode.State.Activated ||
            node.state == SkillTreeNode.State.CannotBeActivated)
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
    public void ActivateNode()
    {
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = skillTreeNodeInfo.displayName;
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
        SkillTreeEvents.ActivateNode(skillTreeNodeInfo.id);
        
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
