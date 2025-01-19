using System.Text;
using Skill;
using Skill.Events;
using Skill.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeButton : MonoBehaviour, ITooltippable
{
    [SerializeField] public SkillTreeNodeSO skillTreeNodeInfo;
    
    private Button _button;
    private Image _image;
    private bool _activated;
    
    private void Awake()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
        _button.onClick.AddListener(ActivateNodeUsingSkillPoint);
    }

    public void OnEnable()
    {
        PlayerResourcesEvents.OnSkillPointsValueChanged += CheckIfNodeCanBeEnabled;
    }
    
    public void OnDisable()
    {
        PlayerResourcesEvents.OnSkillPointsValueChanged -= CheckIfNodeCanBeEnabled;
    }

    public void Start()
    {
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = skillTreeNodeInfo.displayName;
        var node = SkillTreeManager.GetNodeByName(skillTreeNodeInfo.name);
        if (node.state == SkillTreeNode.State.Activated)
        {
            ActivateNode();
        }
        CheckIfNodeCanBeEnabled(PlayerResourcesManager.Instance.SkillPoints);
    }

    private void CheckIfNodeCanBeEnabled(int value)
    {
        if (_activated)
        {
            return;
        }
        
        var node = SkillTreeManager.GetNodeByName(skillTreeNodeInfo.name);
        
        if (node.state == SkillTreeNode.State.CannotBeActivated)
        {
            _button.interactable = false;
            _image.color = Color.gray;
            return;
        }

        if (value > 0)
        {
            _button.interactable = true;
            _image.color = Color.white;
        }
        else
        {
            _button.interactable = false;
            _image.color = Color.gray;
        }
    }
    private void ActivateNode()
    {
        _activated = true;
        _button.interactable = false;
        _image.color = Color.green;
        foreach (var modifier in skillTreeNodeInfo.appliedModifiers)
        {
            PlayerStats.GetInstance().AddSkillTreeModifier(modifier);
        }
        foreach (var ability in skillTreeNodeInfo.unlockedAbilities)
        {
            UnlockedAbilities.GetInstance().UnlockAbility(ability);
        }
    }
    
    private void ActivateNodeUsingSkillPoint()
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
        var str = new StringBuilder();
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
