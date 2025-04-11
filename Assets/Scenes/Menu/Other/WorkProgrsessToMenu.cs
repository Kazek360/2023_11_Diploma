using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorkProgressToMenu : MonoBehaviour
{

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync("Scenes/UI/Menu/MainMenu");
    }
}
