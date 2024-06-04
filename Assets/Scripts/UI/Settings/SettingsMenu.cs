using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject backMenu;

    public void DisableMenu()
    {
        gameObject.SetActive(false);
        backMenu.SetActive(true);
    }
}
