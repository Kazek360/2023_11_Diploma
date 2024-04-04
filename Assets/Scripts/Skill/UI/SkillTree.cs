using TMPro;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    
    [Header("Components")] 
    [SerializeField] private GameObject contentParent;
    [SerializeField] private TextMeshProUGUI skillPointsValue;

    private bool _isAnotherInterfaceOpened;

    public void OnEnable()
    {
        PlayerResourcesEvents.OnSkillPointsChange += ChangeSkillPointsValue;
        InterfaceEvents.OnInterfaceOpened += EnableInterface;
        InterfaceEvents.OnInterfaceClosed += DisableInterface;
    }
    public void OnDisable()
    {
        PlayerResourcesEvents.OnSkillPointsChange -= ChangeSkillPointsValue;
        InterfaceEvents.OnInterfaceOpened -= EnableInterface;
        InterfaceEvents.OnInterfaceClosed -= DisableInterface;
    }
    private void Start()
    {
        _isAnotherInterfaceOpened = false;
        contentParent.SetActive(false);
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
                InterfaceEvents.CloseInterface(gameObject.name);
                contentParent.SetActive(false);
            }
            else
            {
                InterfaceEvents.OpenInterface(gameObject.name);
                contentParent.SetActive(!contentParent.activeSelf);
            }
        }
    }

    private void EnableInterface(string interfaceName)
    {
        if (!interfaceName.Equals(gameObject.name))
        {
            _isAnotherInterfaceOpened = true;
        }
    }
    private void DisableInterface(string interfaceName)
    {
        if (!interfaceName.Equals(gameObject.name))
        {
            _isAnotherInterfaceOpened = false;
        }
    }
    
    private void ChangeSkillPointsValue(int value)
    {
        skillPointsValue.text = value.ToString();
    }
}
