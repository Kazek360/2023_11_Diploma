using System;
using UnityEngine;

public class InterfaceEvents : MonoBehaviour
{
    public static event Action<string> OnInterfaceOpened;
    public static void OpenInterface(string interfaceName)
    {
        OnInterfaceOpened?.Invoke(interfaceName);
    }
    public static event Action<string> OnInterfaceClosed;
    public static void CloseInterface(string interfaceName)
    {
        OnInterfaceClosed?.Invoke(interfaceName);
    }
}
