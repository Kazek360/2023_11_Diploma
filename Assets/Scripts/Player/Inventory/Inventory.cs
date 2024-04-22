using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private InventorySlot[,] _inventorySlots;
    void Awake()
    {
        _inventorySlots = new InventorySlot[5,10];
    }

    public InventorySlot GetSlot(int x, int y)
    {
        return _inventorySlots[x, y];
    }

    public void UseItem(int x, int y)
    {
        _inventorySlots[x, y].Use();
    }

    public void DropItem(int x, int y, int amount)
    {
        _inventorySlots[x, y].RemoveItem(amount);
    }
    
    public void DropItem(int x, int y)
    {
        _inventorySlots[x, y].RemoveItem();
    }

    public void AddItem(int itemID, int amount, int x, int y)
    {
        //TODO: Add items to specific slot
    }
    
    public void AddItem(int itemID, int amount)
    {
        //TODO: Add items to inventory - on pickup for example
    }
}

public class InventorySlot
{
    private int _count = 0;
    private Item _item = null;

    public int Count => _count;
    public Item Item => _item;

    public void Use()
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
     * AddItem służy do zmniejszania ilości przedmiotów w slocie. Zwracana wartość bool daje znać, czy operacja powiodła
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