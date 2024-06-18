using SaveSystem;
using SaveSystem.Data;
using Skill.Events;
using UnityEngine;

public class PlayerResourcesManager : MonoBehaviour, IDataPersistence
{
    public static PlayerResourcesManager Instance { get; private set; }
    public int SkillPoints { get; private set; }
    private void Awake() 
    { 
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }
    public void OnEnable()
    {
        PlayerEvents.OnLevelUp += IncreaseSkillPoints;
        SkillTreeEvents.OnNodeActivation += DecreaseSkillPoints;
    }
    public void OnDisable()
    {
        PlayerEvents.OnLevelUp -= IncreaseSkillPoints;
        SkillTreeEvents.OnNodeActivation -= DecreaseSkillPoints;
    }

    private void IncreaseSkillPoints()
    {
        SkillPoints++;
        PlayerResourcesEvents.ChangeSkillPointsValue(SkillPoints);
    }
    private void DecreaseSkillPoints(string id)
    {
        SkillPoints--;
        PlayerResourcesEvents.ChangeSkillPointsValue(SkillPoints);
    }

    public void LoadData(GameData gameData)
    {
        SkillPoints = gameData.resources["SkillPoints"];
    }

    public void SaveData(ref GameData gameData)
    {
        if (!gameData.resources.TryAdd("SkillPoints", SkillPoints))
        {
            gameData.resources["SkillPoints"] = SkillPoints;
        }
    }
}
