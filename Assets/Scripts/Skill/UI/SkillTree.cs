using TMPro;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillPointsValue;
    
    public void OnEnable()
    {
        PlayerResourcesEvents.OnSkillPointsChange += ChangeSkillPointsValue;
    }
    public void OnDisable()
    {
        PlayerResourcesEvents.OnSkillPointsChange -= ChangeSkillPointsValue;
    }

    private void ChangeSkillPointsValue(int value)
    {
        skillPointsValue.text = value.ToString();
    }
}
