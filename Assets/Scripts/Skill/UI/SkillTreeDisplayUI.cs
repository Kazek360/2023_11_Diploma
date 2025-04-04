using TMPro;
using UnityEngine;

public class SkillTreeDisplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillPoints;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDesc;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
            return;
        }
        skillName.text = skillTreeNodeSo.displayName;
        skillDesc.text = skillTreeNodeSo.description;
    }
}
