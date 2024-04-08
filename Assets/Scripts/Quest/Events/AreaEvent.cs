using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AreaEvent
{
    public static event Action OnAreaEnter;
    public static void EnterArea()
    {
        OnAreaEnter?.Invoke();
    }
}
