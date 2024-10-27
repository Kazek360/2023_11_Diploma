using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Basic container class for an item
 */
public class ItemSlot
{
    protected Item _item = null;
    public Item Item => _item;

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
        if (_item is UsableItem item)
        {
            item.Use();
            if (item.DestroyOnUse)
            {
                _count--;
                if (_count == 0)
                {
                    _item = null;
                }
            }
            return;
        }

        if (_item is EquipmentItem equipmentItem)
        {
            equipmentItem.Equip();
            GameConfig.PlayerInventory.GetEquipmentSlot(equipmentItem.EquipmentSlotType).EquipItem(equipmentItem);
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
    public bool AddItem(Item item, int count)
    {
        if (_item is null)
        {
            _item = item;
            _count = count;
            return true;
        }

        if (_item.ItemID == item.ItemID)
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
    public EquipmentSlot(EquipmentSlotType slotType)
    {
        _slotType = slotType;
    }

    public EquipmentSlotType SlotType => _slotType;
    
    public override void Use()
    {
        PlayerStats.GetInstance().ItemRemoved(_item.ItemID);
        GameConfig.PlayerInventory.AddItem(_item.ItemID, 1);
        if (_item is EquipmentItem equipmentItem)
        {
            equipmentItem.Unequip();
        }
        _item = null;
    }

    public bool EquipItem(EquipmentItem item)
    {
        if (_item is not null)
        {
            PlayerStats.GetInstance().ItemRemoved(_item.ItemID);
            GameConfig.PlayerInventory.AddItem(_item.ItemID, 1);
            if (_item is EquipmentItem equipmentItem)
            {
                equipmentItem.Unequip();
            }
        }
        _item = item;
        return true;
    }

    public void ChangeImagination(int currImagination)
    {
        ((EquipmentItem)_item).ChangeImagination(currImagination);
    }
}