using UnityEngine;
using UnityEngine.SceneManagement; // Potrzebne do zarządzania scenami

public class MainMenu : MonoBehaviour
{
    // Metoda do wywołania po naciśnięciu przycisku
    public void Play()
    {
        // Ładuje scenę o nazwie "Las"
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}