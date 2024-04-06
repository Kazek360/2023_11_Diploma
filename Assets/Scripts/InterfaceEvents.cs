using System;

public static class InterfaceEvents
{
    public static event Action<string> OnInterfaceOpened;
    public static void OpenInterface(string interfaceName)
    {
        OnInterfaceOpened?.Invoke(interfaceName);
    }
    
    public static event Action OnInterfaceClosed;
    public static void CloseInterface()
    {
        OnInterfaceClosed?.Invoke();
    }
}
