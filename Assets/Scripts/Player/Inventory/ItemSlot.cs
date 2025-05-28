using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Basic container class for an item
 */
public class ItemSlot
{
    protected ItemScriptableObject _item = null;
    public ItemScriptableObject Item => _item;

    public virtual void Use(){}
}
/**
 * Slots used in inventory
 */
public class InventorySlot : ItemSlot
{
    private int _count = 0;

    public int Count => _count;

    public override void Use()
    {
        if (_item is null)
        {
            return;
        }
        if (_item.itemType == ItemScriptableObject.ItemType.CONSUMABLE)
        {
            _item.Use();
            if (_item.destroyOnUse)
            {
                _count--;
                if (_count == 0)
                {
                    _item = null;
                }
            }
            return;
        }

        if (_item.itemType == ItemScriptableObject.ItemType.EQUIPMENT)
        {
            _item.Equip();
            GameConfig.PlayerInventory.GetEquipmentSlot(_item.equipmentSlotType).EquipItem(_item);
            _count = 0;
            _item = null;
        }
    }

    /**
     * AddItem adds items to given slot. Operation is possible only when slot has no assigned item or assigned item
     * has the same ID as the added one
     *
     * <param name="item">Object of an item to be added</param>
     * <param name="count">Amount of an item to be added</param>
     * <returns>Whether the operation worked/is even possible</returns>
     */
    public bool AddItem(ItemScriptableObject item, int count)
    {
        if (_item is null)
        {
            _item = item;
            _count = count;
            return true;
        }

        if (_item.name.Equals(item.name))
        {
            _count += count;
        }

        return false;
    }

    /**
     * RemoveItem lowers amount of item in the slot.
     * If amount falls to 0, slot is freed.
     * Operation is possible only if deleted amount is lower or equal to amount in slot.
     * 
     * <param name="count">Amount of an item to be removed</param>
     *<returns>Whether the operation worked/is even possible</returns>
     */
    public bool RemoveItem(int count)
    {
        if (count > _count)
        {
            return false;
        }
        _count -= count;
        if (_count == 0)
        {
            _item = null;
        }
        return true;
    }
    
    public bool RemoveItem()
    {
        _item = null;
        _count = 0;
        return true;
    }
}

/**
 * Slot used for equipment items
 */
public class EquipmentSlot : ItemSlot
{
    private EquipmentSlotType _slotType;
    private int _imaginationInvested = 0;
    public EquipmentSlot(EquipmentSlotType slotType)
    {
        _slotType = slotType;
    }

    public EquipmentSlotType SlotType => _slotType;
    
    public override void Use()
    {
        PlayerStats.GetInstance().ItemRemoved(_item.name);
        GameConfig.PlayerInventory.AddItem(_item.name, 1);
        if (_item.itemType == ItemScriptableObject.ItemType.EQUIPMENT)
        {
            _item.Unequip();
        }
        _item = null;
    }

    public bool EquipItem(ItemScriptableObject item)
    {
        if (_item is not null)
        {
            PlayerStats.GetInstance().ItemRemoved(_item.name);
            GameConfig.PlayerInventory.AddItem(_item.name, 1);
            if (_item.itemType == ItemScriptableObject.ItemType.EQUIPMENT)
            {
                _item.Unequip();
            }
        }
        _item = item;
        return true;
    }

    public void ChangeImagination(int currImagination)
    {
        _imaginationInvested = currImagination;
        _item.ChangeImagination(currImagination);
    }

    public int ImaginationInvested => _imaginationInvested;

    public void SetImagination(int currImagination)
    {
        _imaginationInvested = currImagination;
    }
}