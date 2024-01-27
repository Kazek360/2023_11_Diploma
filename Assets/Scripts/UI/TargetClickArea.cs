using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetClickArea : MonoBehaviour
{
    private IHittable _parent;
    private CombatUI _combatUI;
    [SerializeField] private SpriteRenderer indicator;
    
    public void EnableClick(CombatUI combatUI, IHittable parent)
    {
        //Debug.Log("ACTIVATED");
        gameObject.SetActive(true);
        _combatUI = combatUI;
        _parent = parent;
    }
    
    public void DisableClick()
    {
        gameObject.SetActive(false);
    }
    private void OnMouseDown()
    {
        //Debug.Log("CLICKED");
        _combatUI.SetTarget(_parent);
        indicator.color = Color.green;
    }

    public void Deselect()
    {
        indicator.color = Color.white;
    }
}
