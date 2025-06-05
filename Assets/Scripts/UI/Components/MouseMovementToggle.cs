using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
class MouseMovementToggle : MonoBehaviour
{
    private Toggle _toggle;


    private void Awake()
    {
        _toggle = gameObject.GetComponent<Toggle>();
    }

    private void Start()
    {
        _toggle.isOn = SettingsDataHandler.Instance.MouseMovement;
    }
    
    public void ChangeValue()
    {
        SettingsDataHandler.Instance.MouseMovement = _toggle.isOn;
    }
}
