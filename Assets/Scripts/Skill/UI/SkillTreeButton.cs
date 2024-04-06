using Skill;
using Skill.Events;
using Skill.Managers;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeButton : MonoBehaviour
{
    [SerializeField] public Button button;
    [SerializeField] public SkillTreeNodeSO skillTreeNodeInfo;

    public void OnEnable()
    {
        PlayerResourcesEvents.OnSkillPointsChange += CheckIfNodeCanBeEnabled;
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
        button.enabled = false;
        button.image.color = Color.cyan;
        SkillTreeEvents.ActivateNode(skillTreeNodeInfo.id);
    }
}
