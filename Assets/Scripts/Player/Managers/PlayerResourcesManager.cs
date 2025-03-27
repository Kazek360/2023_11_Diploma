using SaveSystem;
using SaveSystem.Data;
using SaveSystem.Interfaces;
using Shared;
using Skill.Events;

public class PlayerResourcesManager : Singleton<PlayerResourcesManager>, ISavable
{
    private int _skillpoints;
    public int SkillPoints
    {
        get => _skillpoints;
        set
        {
            _skillpoints = value;
            PlayerResourcesEvents.InvokeOnSkillPointsValueChanged(value);
        }
    }
    
    public void OnEnable()
    {
        SaveSystemEvents.OnFromFileLoad += LoadData;
        SaveSystemEvents.OnBetweenScenesLoad += LoadData;
        SaveSystemEvents.OnSave += SaveData;
        
        PlayerEvents.OnLevelUp += IncreaseSkillPoints;
        SkillTreeEvents.OnNodeActivation += DecreaseSkillPoints;
    }
    public void OnDisable()
    {
        SaveSystemEvents.OnFromFileLoad -= LoadData;
        SaveSystemEvents.OnBetweenScenesLoad -= LoadData;
        SaveSystemEvents.OnSave -= SaveData;
        
        PlayerEvents.OnLevelUp -= IncreaseSkillPoints;
        SkillTreeEvents.OnNodeActivation -= DecreaseSkillPoints;
    }

    private void IncreaseSkillPoints()
    {
        SkillPoints++;
    }
    private void DecreaseSkillPoints(string id)
    {
        SkillPoints--;
    }

    public void LoadData(GameData gameData)
    {
        PlayerStats.GetInstance();
        SkillPoints = gameData.resources["SkillPoints"];
        PlayerStats.GetInstance().Level = gameData.resources["Level"];
        PlayerStats.GetInstance().CurrentXp = gameData.resources["ExperiencePoints"];
        PlayerStats.GetInstance().WitHp = gameData.resources["WitHP"];
        PlayerStats.GetInstance().NestorHp = gameData.resources["NestorHP"];
    }

    public void SaveData(GameData gameData)
    {
        if (!gameData.resources.TryAdd("SkillPoints", SkillPoints))
        {
            gameData.resources["SkillPoints"] = SkillPoints;
        }
        if (!gameData.resources.TryAdd("WitHP", PlayerStats.GetInstance().WitHp))
        {
            gameData.resources["WitHP"] = PlayerStats.GetInstance().WitHp;
        }
        if (!gameData.resources.TryAdd("NestorHP", PlayerStats.GetInstance().NestorHp))
        {
            gameData.resources["NestorHP"] = PlayerStats.GetInstance().NestorHp;
        }
        if (!gameData.resources.TryAdd("Level", PlayerStats.GetInstance().Level))
        {
            gameData.resources["SkillPoints"] = PlayerStats.GetInstance().Level;
        }
        if (!gameData.resources.TryAdd("ExperiencePoints", PlayerStats.GetInstance().CurrentXp))
        {
            gameData.resources["SkillPoints"] = PlayerStats.GetInstance().CurrentXp;
        }
    }
}
