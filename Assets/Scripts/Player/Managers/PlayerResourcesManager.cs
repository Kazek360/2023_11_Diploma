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
    
    // private void DecreaseSkillPoints(string id)
    // {
    //     SkillPoints--;
    // }
    
    private void DecreaseSkillPoints(string id, int skillPoints)
    {
        SkillPoints -= skillPoints;
    }

    public void LoadData(GameData gameData)
    {
        PlayerStats.GetInstance();
        PlayerStats.GetInstance().Level = gameData.resources.level;
        PlayerStats.GetInstance().CurrentXp = gameData.resources.currentExp;
        SkillPoints = gameData.resources.skillPoints;
        PlayerStats.GetInstance().WitHp = gameData.resources.witHp;
        PlayerStats.GetInstance().NestorHp = gameData.resources.nestorHp;
    }

    public void SaveData(GameData gameData)
    {
        if (gameData == null)
        {
            return;
        }

        gameData.resources.level = PlayerStats.GetInstance().Level;
        gameData.resources.currentExp = PlayerStats.GetInstance().CurrentXp;
        gameData.resources.skillPoints = SkillPoints;
        gameData.resources.witHp = PlayerStats.GetInstance().WitHp;
        gameData.resources.nestorHp = PlayerStats.GetInstance().NestorHp;
    }
}
