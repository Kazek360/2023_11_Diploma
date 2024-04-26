using System;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentSlotType {head, torso, hand, legs, feet}

public class Item
{
    private int _itemID;
    private string _itemName;
    private string _itemDesc;
    private Sprite _itemSprite;
    private int _maxStackSize;

    public Item(int itemID, string itemName, string itemDesc, Sprite itemSprite, int maxStackSize)
    {
        _itemID = itemID;
        _itemName = itemName;
        _itemDesc = itemDesc;
        _itemSprite = itemSprite;
        _maxStackSize = maxStackSize;
    }

    public int ItemID => _itemID;

    public string ItemName => _itemName;

    public string ItemDesc => _itemDesc;

    public Sprite ItemSprite => _itemSprite;

    public int MaxStackSize => _maxStackSize;
}


public class EquipmentItem: Item
{
    private Action _equip;
    private EquipmentSlotType _equipmentSlotType;
    private List<ItemStatModifier> _modifiers;

    public EquipmentItem(int itemID, string itemName, string itemDesc, Sprite itemSprite, Action equip, EquipmentSlotType equipmentSlotType, List<ItemStatModifier> modifiers) : base(itemID, itemName, itemDesc, itemSprite, 1)
    {
        _equip = equip;
        _equipmentSlotType = equipmentSlotType;
        _modifiers = modifiers;
    }

    public void Equip()
    {
        foreach (var modifier in _modifiers)
        {
            GameConfig.PlayerStats.AddItemModifier(modifier);
        }
        _equip();
    }

    public EquipmentSlotType EquipmentSlotType => _equipmentSlotType;
}

public class UsableItem: Item
{
    private Action _use;
    private bool _destroyOnUse;

    public UsableItem(int itemID, string itemName, string itemDesc, Sprite itemSprite, int maxStackSize, Action use, bool destroyOnUse) : base(itemID, itemName, itemDesc, itemSprite, maxStackSize)
    {
        _use = use;
        _destroyOnUse = destroyOnUse;
    }

    public void Use()
    {
        _use();
    }

    public bool DestroyOnUse => _destroyOnUse;
}