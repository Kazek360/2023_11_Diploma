using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemSlot
{
    protected Item _item = null;
    public Item Item => _item;

    public virtual void Use(){}
}

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
     * AddItem służy do dodawania przedmiotu do wybranego Slotu.Zwracana wartość bool daje znać, czy operacja powiodła
     * się / jest możliwa.
     *
     * Operacja dodania przedmiotu jest możliwa tylko wtedy, gdy slot nie ma przypisanego przedmiotu, lub przypisany
     * przedmiot jest taki sam jak dodawany.
     * 
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
     * RemoveItem służy do zmniejszania ilości przedmiotów w slocie. Zwracana wartość bool daje znać, czy operacja powiodła
     * się / jest możliwa.
     *
     * Jeśli po usunięciu ilość przedmiotów w slocie spadnie do zera, jest on zwalniany.
     *
     * Operacja jest możliwa tylko wtedy, gdy usuwana ilość jest mniejsza lub równa ilości w slocie.
     *
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
        ((EquipmentItem)_item).Unequip();
        _item = null;
    }

    public bool EquipItem(EquipmentItem item)
    {
        if (_item is not null)
        {
            PlayerStats.GetInstance().ItemRemoved(_item.ItemID);
            GameConfig.PlayerInventory.AddItem(_item.ItemID, 1);
            ((EquipmentItem)_item).Unequip();
        }
        _item = item;
        return true;
    }
}