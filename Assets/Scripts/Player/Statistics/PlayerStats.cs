using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerStats
{
    private static PlayerStats _instance;

    public static PlayerStats GetInstance()
    {
        if (_instance is null)
        {
            _instance = new PlayerStats();
        }

        return _instance;
    }

    private PlayerStats()
    {
        Load();
    }
    
    private List<ItemStatModifier> _itemStatModifiers;
    private List<SkillTreeStatModifier> _skillTreeStatModifiers;
    private List<BaseStat> _baseStats;
    
    private int _witHp;
    private int _nestorHp;
    private int _freeImagination;   //TODO: Tell Oskar to integrate it with saves
    private int _level;
    private int _currentXp;

    public int GetBaseStat(StatType statType)
    {
        foreach (var baseStat in _baseStats)
        {
            if (baseStat.StatType == statType)
            {
                return baseStat.Value;
            }
        }

        return -1;
    }

    public int GetStat(StatType statType)
    {
        int res = GetBaseStat(statType);
        foreach (var modifier in _itemStatModifiers)
        {
            if (modifier.StatType == statType)
            {
                res += modifier.Modifier;
            }
        }
        foreach (var modifier in _skillTreeStatModifiers)
        {
            if (modifier.StatType == statType)
            {
                res += modifier.Modifier;
            }
        }

        return res;
    }

    public void ItemRemoved(int itemID)
    {
        for (int i = _itemStatModifiers.Count - 1; i >= 0; i--)
        {
            if (_itemStatModifiers[i].ItemID == itemID)
            {
                _itemStatModifiers.RemoveAt(i);
            }
        }
    }

    public void AddItemModifier(ItemStatModifier modifier)
    {
        _itemStatModifiers.Add(modifier);
    }
    
    public void SkillRemoved(string skillID)
    {
        for (int i = _skillTreeStatModifiers.Count - 1; i >= 0; i--)
        {
            if (_skillTreeStatModifiers[i].SkillID == skillID)
            {
                _itemStatModifiers.RemoveAt(i);
            }
        }
    }
    
    public void AddSkillTreeModifier(SkillTreeStatModifier modifier)
    {
        _skillTreeStatModifiers.Add(modifier);
        if (modifier.StatType == StatType.WitHealth)
        {
            _witHp += modifier.Modifier;
        }
    }

    public int WitHp
    {
        get => _witHp;
        set => _witHp = value;
    }

    public int NestorHp
    {
        get => _nestorHp;
        set => _nestorHp = value;
    }

    public int Level
    {
        get => _level;
        set => _level = value;
    }

    public int CurrentXp
    {
        get => _currentXp;
        set => _currentXp = value;
    }

    public int FreeImagination
    {
        get => _freeImagination;
        set => _freeImagination = value;
    }

    public void AddXp(int xp)
    {
        _currentXp += xp;
        if (_currentXp >= 1000)
        {
            PlayerEvents.LevelUp();
            _level++;
            _currentXp -= 1000;
        }
    }
    
    public void Load()
    {
        _level = 1;
        _itemStatModifiers = new List<ItemStatModifier>();
        _skillTreeStatModifiers = new List<SkillTreeStatModifier>();
        _baseStats = new List<BaseStat>();
        
        BasicSetup();

        _witHp = GetStat(StatType.WitHealth);
        _nestorHp = GetStat(StatType.NestorHealth);
        _freeImagination = GetStat(StatType.WitImagination);
    }

    /**
     * Inserts default skill values into stat containers
     */
    private void BasicSetup()
    {
        _baseStats.Add(new BaseStat(StatType.WitAgility, 2));
        _baseStats.Add(new BaseStat(StatType.WitStrength, 3));
        _baseStats.Add(new BaseStat(StatType.WitLuck, 3));
        _baseStats.Add(new BaseStat(StatType.WitImagination, 2));
        _baseStats.Add(new BaseStat(StatType.WitHealth, 8));
        _baseStats.Add(new BaseStat(StatType.NestorAgility, 3));
        _baseStats.Add(new BaseStat(StatType.NestorStrength, 4));
        _baseStats.Add(new BaseStat(StatType.NestorLuck, 1));
        _baseStats.Add(new BaseStat(StatType.NestorImagination, 2));
        _baseStats.Add(new BaseStat(StatType.NestorHealth, 10));
    }

    /**
     * Prints all player stats values.
     * Useful for debugging.
     */
    public string GetPlayerStatsString()
    {
        StringBuilder stringBuilder = new StringBuilder();

        foreach(StatType statType in Enum.GetValues(typeof(StatType)))
        {
            stringBuilder.Append(statType + ": " + GetStat(statType) + "\n");
        }

        return stringBuilder.ToString();
    }

    /**
     * Turn a StatType enum into a normal string
     *
     * <param name="statType">StatType enum to be turned into a String</param>
     * <returns>A string representation of a stat type.</returns>
     */
    public static string TranslateStatType(StatType statType)
    {
        switch (statType)
        {
            case StatType.WitHealth: return "Życie Wita";
            case StatType.WitStrength: return "Siła Wita";
            case StatType.WitAgility: return "Zręczność Wita";
            case StatType.WitLuck: return "Szczęście Wita";
            case StatType.WitImagination: return "Wyobraźnia Wita";
            case StatType.NestorHealth: return "Życie Nestora";
            case StatType.NestorStrength: return "Siła Nestora";
            case StatType.NestorAgility: return "Zręczność Nestora";
            case StatType.NestorLuck: return "Szczęście Nestora";
            case StatType.NestorImagination: return "Wyobraźnia Nestora";
            default: return "STAT_NAME_NOT_FOUND";
        }
        
    }

    public int GetWitHp()
    {
        return _witHp;
    }
    
    public void SetWitHp(int val)
    {
        _witHp = val;
    }
    
    
    public void ModifyWitHealth(int val)
    {
        _witHp = Math.Min(Math.Max(0, _witHp + val), GetStat(StatType.WitHealth));
    }
    
    public int GetNestorHp()
    {
        return _nestorHp;
    }
    
    public void SetNestorHp(int val)
    {
        _nestorHp = val;
    }

    public void ModifyNestorHealth(int val)
    {
        _nestorHp = Math.Min(Math.Max(0, _nestorHp + val), GetStat(StatType.NestorHealth));
    }
}

public enum StatType
{
    WitHealth, WitStrength, WitAgility, WitLuck, WitImagination,
    NestorHealth, NestorStrength, NestorAgility, NestorLuck, NestorImagination
}

[Serializable]
public class ItemStatModifier
{
    [SerializeField]private StatType _statType;
    [SerializeField]private int _modifier;
    [SerializeField]private int _itemID;

    public ItemStatModifier(StatType statType, int modifier, int itemID)
    {
        _statType = statType;
        _modifier = modifier;
        _itemID = itemID;
    }

    public StatType StatType => _statType;

    public int Modifier => _modifier;

    public int ItemID => _itemID;

    public string StatString()
    {
        if (_modifier > 0)
        {
            return "+" + _modifier + " " + PlayerStats.TranslateStatType(_statType);
        }
        return _modifier + " " + PlayerStats.TranslateStatType(_statType);
    }
}

[System.Serializable]
public class SkillTreeStatModifier
{

    [SerializeField] private StatType _statType;
    [SerializeField] private int _modifier;
    private string _skillID;

    public SkillTreeStatModifier(StatType statType, int modifier)
    {
        _statType = statType;
        _modifier = modifier;
    }

    public StatType StatType => _statType;

    public int Modifier => _modifier;

    public string SkillID
    {
        get => _skillID;
        set => _skillID = value;
    }

    public string StatString()
    {
        if (_modifier > 0)
        {
            return "+" + _modifier + " " + PlayerStats.TranslateStatType(_statType);
        }
        return _modifier + " " + PlayerStats.TranslateStatType(_statType);
    }
}

public class BaseStat
{
    private readonly StatType _statType;
    private readonly int _value;

    public BaseStat(StatType statType, int value)
    {
        _statType = statType;
        _value = value;
    }

    public StatType StatType => _statType;

    public int Value => _value;
}