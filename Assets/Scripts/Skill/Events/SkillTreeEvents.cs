using System;
using UnityEngine;

public class SkillTreeEvents : MonoBehaviour
{
    public static event Action<string> OnNodeActivation;
    public static void ActivateNode(string id)
    {
        OnNodeActivation?.Invoke(id);
    }
    
    public static event Action<string> OnNodeEnabled;
    public static void EnableNode(string id)
    {
        OnNodeEnabled?.Invoke(id);
    }
}
