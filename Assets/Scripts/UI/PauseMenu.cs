using System;
using InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject contentParent;
    private void OnEnable()
    {
        InputEvents.OnPauseToggle += TogglePause;
    }
    private void OnDisable()
    {
        InputEvents.OnPauseToggle -= TogglePause;
    }

    private void Start()
    {
        contentParent.SetActive(false);
    }

    private void TogglePause()
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
    
    public void QuitGame()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
