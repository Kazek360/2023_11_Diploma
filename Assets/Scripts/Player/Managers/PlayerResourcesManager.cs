using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResourcesManager : MonoBehaviour
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
}
