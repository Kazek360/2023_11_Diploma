using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Placeholder class, the only functionality   of which is assigning this canvas to GameConfigLib
 */
public class AssignUI : MonoBehaviour
{
    public void OnEnable()
    {
        GameConfigLib.canvas = GetComponent<Canvas>();
    }
}
