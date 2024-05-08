using SaveSystem;
using SaveSystem.Data;
using UnityEngine;

public class NpcPosition : MonoBehaviour, IDataPersistence
{
    
    public void LoadData(GameData gameData)
    {
        transform.position = new Vector3(gameData.hrycPosition[0], gameData.hrycPosition[1], 0);
    }

    public void SaveData(ref GameData gameData)
    { 
        gameData.hrycPosition = new [] {transform.position.x, transform.position.y};
    }
}
