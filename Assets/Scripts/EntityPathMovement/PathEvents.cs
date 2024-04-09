using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathEvents
{
    public static event Action OnMovementStarted;
    public static void StartMovement()
    {
        OnMovementStarted?.Invoke();
    }
}
