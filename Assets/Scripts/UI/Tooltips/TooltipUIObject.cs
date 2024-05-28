using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipUIObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ITooltippable _target;
    private bool _triggered;
    private TooltipUI _tooltip;
    [SerializeField] private GameObject TooltipPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        _target = GetComponentInParent<ITooltippable>();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_triggered)
        {
            return;
        }

        _tooltip = Instantiate(TooltipPrefab).GetComponent<TooltipUI>();
        if (_target is not null)
        {
            _tooltip.Setup(_target.GetTooltipHeader(), _target.GetTooltipDesc());
        }

        _triggered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _triggered = false;
        _tooltip.Delete();
    }
    
    public void OnDisable()
    {
        if (_tooltip is not null)
        {
            Destroy(_tooltip.gameObject);
        }
    }
}
