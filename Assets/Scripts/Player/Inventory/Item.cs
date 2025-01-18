using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum EquipmentSlotType {head, torso, hand, legs, feet}

/**
 * Class defining a basic item
 */
public class Item
{
    private string _itemID;
    private string _itemName;
    protected string _itemDesc;
    private List<Sprite> _itemSprites;
    private int _maxStackSize;

    public Item(string itemID, string itemName, string itemDesc, List<Sprite> itemSprites, int maxStackSize)
    {
        _itemID = itemID;
        _itemName = itemName;
        _itemDesc = itemDesc;
        _itemSprites = itemSprites;
        _maxStackSize = maxStackSize;
    }

    public string ItemID => _itemID;

    public string ItemName => _itemName;

    /**
     *
     * 
     * <returns>Item description string</returns>
     */
    public virtual string ItemDesc()
    {
        return _itemDesc;
    }

    public Sprite ItemSprite()
    {
        return _itemSprites[0];
    }

    public Sprite ItemSprite(int imaginationLevel)
    {
        return _itemSprites[Math.Min(imaginationLevel, _itemSprites.Count - 1)];
    }

    public int MaxStackSize => _maxStackSize;
}

/**
 * Class defining an item which can be equipped in equipment slot
 */
public class EquipmentItem: Item
{
    private Action _equip, _unequip;
    private EquipmentSlotType _equipmentSlotType;
    private List<ItemStatModifier> _modifiers;
    private List<ItemStatModifier> _imaginationModifiers;

    public EquipmentItem(string itemID, string itemName, string itemDesc, List<Sprite> itemSprites, Action equip, Action unequip, 
        EquipmentSlotType equipmentSlotType, List<ItemStatModifier> modifiers, List<ItemStatModifier> imaginationModifiers) : base(itemID, itemName, itemDesc, itemSprites, 1)
    {
        _equip = equip;
        _unequip = unequip;
        _equipmentSlotType = equipmentSlotType;
        _modifiers = modifiers;
        _imaginationModifiers = imaginationModifiers;
    }
    /**
     * Item adds all appropriate modifiers and performs equip behaviour
     */
    public void Equip()
    {
        foreach (var modifier in _modifiers)
        {
            PlayerStats.GetInstance().AddItemModifier(modifier);
        }
        _equip();
    }
    /**
    * Item removes all appropriate modifiers and performs unequip behaviour
    */
    public void Unequip()
    {
        _unequip();
    }

    public override string ItemDesc()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(_itemDesc + "\n");
        foreach (var modifier in _modifiers)
        {
            builder.Append(modifier.StatString() + "\n");
        }
        builder.Append("Na jednostkę wyobraźni:\n");
        foreach (var modifier in _imaginationModifiers)
        {
            builder.Append(modifier.StatString() + "\n");
        }
        return builder.ToString();
    }

    public void ChangeImagination(int currImagination)
    {
        PlayerStats.GetInstance().ItemRemoved(ItemID);
        foreach (var modifier in _modifiers)
        {
            PlayerStats.GetInstance().AddItemModifier(modifier);
        }

        for (int i = 0; i < currImagination; i++)
        {
            foreach (var modifier in _imaginationModifiers)
            {
                PlayerStats.GetInstance().AddItemModifier(modifier);
            }
        }
    }

    public EquipmentSlotType EquipmentSlotType => _equipmentSlotType;
}

/**
 * Class defining an item which can be used in inventory
 */
public class UsableItem: Item
{
    private Action _use;
    private bool _destroyOnUse;

    public UsableItem(string itemID, string itemName, string itemDesc, List<Sprite> itemSprites, int maxStackSize, Action use, bool destroyOnUse) : base(itemID, itemName, itemDesc, itemSprites, maxStackSize)
    {
        _use = use;
        _destroyOnUse = destroyOnUse;
    }

    /**
     * Behaviour of an item when used
     */
    public void Use()
    {
        _use();
    }

    public bool DestroyOnUse => _destroyOnUse;
}