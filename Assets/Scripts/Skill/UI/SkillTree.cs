using InputSystem;
using TMPro;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillPointsValue;
    [SerializeField] private GameObject contentParent;
    
    public void OnEnable()
    {
        InputEvents.OnSkillTreeToggle += ToggleSkillTree;
        InputEvents.OnInterfaceClosed += CloseSkillTree;
        
        PlayerResourcesEvents.OnSkillPointsChange += ChangeSkillPointsValue;
    }
    public void OnDisable()
    {
        InputEvents.OnSkillTreeToggle -= ToggleSkillTree;
        InputEvents.OnInterfaceClosed -= CloseSkillTree;
        
        PlayerResourcesEvents.OnSkillPointsChange -= ChangeSkillPointsValue;
    }
    private void Start()
    {
        contentParent.SetActive(false);
    }

    private void ToggleSkillTree()
    {
        if (contentParent.activeSelf)
        {
            contentParent.SetActive(false);
            GameConfig.QuestTracker.EnableText();
        }
        else
        {
            contentParent.SetActive(true);
            GameConfig.QuestTracker.DisableText();
        }
    }
    
    private void CloseSkillTree()
    {
        contentParent.SetActive(false);
    }
    
    private void ChangeSkillPointsValue(int value)
    {
        skillPointsValue.text = value.ToString();
    }
}
