using InputSystem;
using TMPro;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillPointsValue;
    [SerializeField] private GameObject contentParent;
    private bool _isAnotherInterfaceOpened;
    
    public void OnEnable()
    {
        PlayerResourcesEvents.OnSkillPointsChange += ChangeSkillPointsValue;
    }
    public void OnDisable()
    {
        PlayerResourcesEvents.OnSkillPointsChange -= ChangeSkillPointsValue;
    }
    private void Start()
    {
        contentParent.SetActive(false);
        _isAnotherInterfaceOpened = false;
    }
    private void Update()
    {
        if (_isAnotherInterfaceOpened)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (contentParent.activeSelf)
            {
                contentParent.SetActive(false);
            }
            else
            {
                contentParent.SetActive(true);
                InputEvents.OpenInterface();
            }
        }
    }
    private void ChangeSkillPointsValue(int value)
    {
        skillPointsValue.text = value.ToString();
    }
}
