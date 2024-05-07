using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Placeholder class, the only functionality of which is assigning this canvas to GameConfig
 */
public class AssignUI : MonoBehaviour
{
    public void OnEnable()
    {
        GameConfig.canvas = GetComponent<Canvas>();
    }
}
