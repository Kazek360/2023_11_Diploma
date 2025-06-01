using System;
using Unity.VisualScripting;
using UnityEngine;

public class TooltipObject : MonoBehaviour
{
    private ITooltippable _target;
    private bool _triggered;
    private bool _mouseIsOver;
    private TooltipUI _tooltip;
    [SerializeField] private GameObject TooltipPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        _mouseIsOver = false;
        _target = GetComponentInParent<ITooltippable>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(2) && _mouseIsOver)
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

    // Update is called once per frame
    void OnMouseOver()
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

    private void OnMouseExit()
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
