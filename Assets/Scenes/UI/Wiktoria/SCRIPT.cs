using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SCRIPT : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("1");
    }

    public void Main()
    {
        SceneManager.LoadSceneAsync("2");
    }

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync("MAIN MENU");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
