using InputSystem;
using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    [SerializeField] private GameObject contentParent;
    
    public void OnEnable()
    {
        InputEvents.OnInventoryToggle += ToggleInventory;
        InputEvents.OnInterfaceClosed += CloseInventory;
    }
    
    public void OnDisable()
    {
        InputEvents.OnInventoryToggle += ToggleInventory;
        InputEvents.OnInterfaceClosed += CloseInventory;
    }
    
    private void ToggleInventory()
    {
        if (contentParent.activeSelf)
        {
            contentParent.SetActive(false);
        }
        else
        {
            contentParent.SetActive(true);
        }
    }
    
    private void CloseInventory()
    {
        contentParent.SetActive(false);
    }
}
