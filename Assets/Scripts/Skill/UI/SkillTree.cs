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
        InterfaceEvents.OnInterfaceOpened += DisableInterface;
        InterfaceEvents.OnInterfaceClosed += EnableInterface;
    }
    public void OnDisable()
    {
        PlayerResourcesEvents.OnSkillPointsChange -= ChangeSkillPointsValue;
        InterfaceEvents.OnInterfaceOpened -= DisableInterface;
        InterfaceEvents.OnInterfaceClosed -= EnableInterface;
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
                InterfaceEvents.CloseInterface();
            }
            else
            {
                contentParent.SetActive(true);
                InterfaceEvents.OpenInterface(gameObject.name);
            }
        }
    }
    private void DisableInterface(string interfaceName)
    {
        if (!interfaceName.Equals(gameObject.name))
        {
            _isAnotherInterfaceOpened = true;
        }
    }

    private void EnableInterface()
    {
        _isAnotherInterfaceOpened = false;
    }
    private void ChangeSkillPointsValue(int value)
    {
        skillPointsValue.text = value.ToString();
    }
}
