using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipObject : MonoBehaviour
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

    // Update is called once per frame
    void OnMouseOver()
    {
        if (_triggered)
        {
            return;
        }

        _tooltip = Instantiate(TooltipPrefab).GetComponent<TooltipUI>();
        _tooltip.Setup(_target.GetTooltipHeader(), _target.GetTooltipDesc());

        _triggered = true;
        print (gameObject.name);
        //TODO SPAWN TOOLTIP
    }

    private void OnMouseExit()
    {
        _triggered = false;
        _tooltip.Delete();
        print(gameObject.name + " OVER");
    }
}
