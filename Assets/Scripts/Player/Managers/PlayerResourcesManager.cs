using SaveSystem;
using SaveSystem.Data;
using Skill.Events;
using UnityEngine;

public class PlayerResourcesManager : MonoBehaviour, IDataPersistence
{
    public static PlayerResourcesManager Instance { get; private set; }
    
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
    }
    private void DecreaseSkillPoints(string id)
    {
        SkillPoints--;
    }

    public void LoadData(GameData gameData)
    {
        if (DataPersistenceManager.Instance.CurrentLoadMode == LoadMode.NewGame)
        {
            return;
        }
        PlayerStats.GetInstance();
        SkillPoints = gameData.resources["SkillPoints"];
        PlayerStats.GetInstance().Level = gameData.resources["Level"];
        PlayerStats.GetInstance().CurrentXp = gameData.resources["ExperiencePoints"];
        PlayerStats.GetInstance().WitHp = gameData.resources["WitHP"];
        PlayerStats.GetInstance().NestorHp = gameData.resources["NestorHP"];
    }

    public void SaveData(ref GameData gameData)
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
