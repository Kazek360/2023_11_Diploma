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
        Toggle toggled = _toggleGroup.GetFirstActiveToggle();
        if (toggled is null)
        {
            _skillTreeDisplay.SetDisplayedSkill(null);
            return;
        }

        SkillTreeNodeUI nodeUI = toggled.gameObject.GetComponent<SkillTreeNodeUI>();
        _skillTreeDisplay.SetDisplayedSkill(nodeUI);
    }

    public void Deselect()
    {
        _toggleGroup.SetAllTogglesOff();
    }
}
