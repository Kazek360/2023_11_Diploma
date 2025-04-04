using UnityEngine;
using UnityEngine.UI;

public class SkillTreeFacade : MonoBehaviour
{
    private ToggleGroup _toggleGroup;

    [SerializeField] private SkillTreeDisplayUI _skillTreeDisplay;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _toggleGroup = GetComponent<ToggleGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleChosenNode()
    {
        
        Debug.Log("A2");
        SkillTreeNodeSO chosenSkill = _toggleGroup.GetFirstActiveToggle().GetComponent<SkillTreeNodeUI>().skillTreeNodeInfo;
        _skillTreeDisplay.SetDisplayedSkill(chosenSkill);
    }
}
