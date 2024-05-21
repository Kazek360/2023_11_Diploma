using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    private List<ItemStatModifier> _itemStatModifiers;
    private List<BaseStat> _baseStats;
    // Start is called before the first frame update
    void OnEnable()
    {
        GameConfig.PlayerStats = this;
        Load();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

    public List<ItemStatModifier> GetModifiers(StatType statType)
    {
        List<ItemStatModifier> _tmpMod = new List<ItemStatModifier>();
        foreach (ItemStatModifier modifier in _itemStatModifiers)
        {
            if (modifier.StatType == statType)
            {
                _tmpMod.Add(modifier);
            }
        }

        return _tmpMod;
    }
    
    public List<ItemStatModifier> GetModifiers()
    {
        return _itemStatModifiers;
    }
    
    

    public void Save()
    {
        //TODO: To be done by Oskar during data persistence development
    }
    
    public void Load()
    {
        //TODO: To be done by Oskar during data persistence development

        _itemStatModifiers = new List<ItemStatModifier>();
        _baseStats = new List<BaseStat>();
        
        _baseStats.Add(new BaseStat(StatType.Agility, 2));
        _baseStats.Add(new BaseStat(StatType.Strength, 3));
        _baseStats.Add(new BaseStat(StatType.Luck, 3));
        _baseStats.Add(new BaseStat(StatType.Imagination, 2));
        _baseStats.Add(new BaseStat(StatType.Health, 8));
    }

    public string GetPlayerStatsString()
    {
        StringBuilder stringBuilder = new StringBuilder();

        foreach(StatType statType in Enum.GetValues(typeof(StatType)))
        {
            stringBuilder.Append(statType + ": " + GetStat(statType) + "\n");
        }

        return stringBuilder.ToString();
    }

    public static string TranslateStatType(StatType statType)
    {
        switch (statType)
        {
            case StatType.Health: return "Życie";
            case StatType.Strength: return "Siła";
            case StatType.Agility: return "Zręczność";
            case StatType.Luck: return "Szczęście";
            case StatType.Imagination: return "Wyobraźnia";
            default: return "STAT_NAME_NOT_FOUND";
        }
        
    }
}
public enum StatType{Health, Strength, Agility, Luck, Imagination}

public class ItemStatModifier
{
    private readonly StatType _statType;
    private readonly int _modifier;
    private readonly int _itemID;

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