using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipUIObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ITooltippable _target;
    private bool _triggered;
    private bool _mouseIsOver;
    private TooltipUI _tooltip;
    [SerializeField] private GameObject TooltipPrefab;
    
    void Start()
    {
        _mouseIsOver = false;
        _target = GetComponentInParent<ITooltippable>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && _mouseIsOver)
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
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        _mouseIsOver = true;
        // if (_triggered)
        // {
        //     return;
        // }
        //
        // _tooltip = Instantiate(TooltipPrefab).GetComponent<TooltipUI>();
        // if (_target is not null)
        // {
        //     _tooltip.Setup(_target.GetTooltipHeader(), _target.GetTooltipDesc());
        // }
        //
        // _triggered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _mouseIsOver = false;
        _triggered = false;
        if (_tooltip is not null)
        {
            _tooltip.Delete();
            _tooltip = null;
        }
    }
    
    public void OnDisable()
    {
        if (_tooltip is null || _tooltip.IsDestroyed())
        {
            return;
        }
        if (_tooltip.isActiveAndEnabled)
        {
            Destroy(_tooltip.gameObject);
        }
    }
}
