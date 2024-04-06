using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerEvents
{
    public static event Action OnLevelUp;
    public static void LevelUp()
    {
        OnLevelUp?.Invoke();
    }
}
