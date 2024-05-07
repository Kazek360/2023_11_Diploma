using SaveSystem;
using SaveSystem.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrentLocation : MonoBehaviour, IDataPersistence
{
    private string _currentScene;

    private void Start()
    {
        _currentScene = SceneManager.GetActiveScene().name;
    }
    
    public void LoadData(GameData gameData)
    {
        _currentScene = gameData.currentScene;
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.currentScene = _currentScene;
    }
}
