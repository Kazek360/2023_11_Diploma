using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private static Inventory _instance;
    private ItemLibrary _itemLibrary;

    public static Inventory GetInstance()
    {
        if (_instance is null)
        {
            _instance = new Inventory();
        }
        return _instance;
    }
    
    private InventorySlot[,] _inventorySlots;
    private Inventory()
    {
        _itemLibrary = ItemLibrary.GetInstance();
        _inventorySlots = new InventorySlot[7,5];
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                _inventorySlots[i, j] = new InventorySlot();
            }
        }
        _inventorySlots[0, 0].AddItem(_itemLibrary.GetItem(0), 2);
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
        bool success = _inventorySlots[x, y].AddItem(_itemLibrary.GetItem(itemID), amount);
        if (!success)
        {
            throw new Exception("DODANIE PRZEDMIOTU NIE POWIODŁO SIĘ");
        }
    }
    
    /**
     * Funkcja służąca do dodawania do ekwipunku wielu identycznych przedmiotów.
     *
     * Używana w przypadku dodawania przedmiotu do ekwipunku.
     *
     * Zwracana liczba to ilość przedmiotów,
     * które nie zostały dodane, np. z powodu braku miejsc.
     */
    public int AddItem(int itemID, int amount)
    {
        Item item = _itemLibrary.GetItem(itemID);
        //TODO: Add items to inventory - on pickup for example
        foreach (InventorySlot inventorySlot in _inventorySlots)
        {
            if (amount == 0)
            {
                Debug.Log("ALL ITEMS ADDED");
                break;
            }
            if (inventorySlot.Item is null)
            {
                int toAdd = Math.Max(amount, item.MaxStackSize);
                inventorySlot.AddItem(item, Math.Min(amount, item.MaxStackSize));
                amount -= toAdd;
                continue;
            }

            if (inventorySlot.Item.ItemID == itemID)
            {
                int toAdd = Math.Min(amount, item.MaxStackSize - inventorySlot.Count);
                inventorySlot.AddItem(item, Math.Max(amount, item.MaxStackSize));
                amount -= toAdd;
                continue;
            }
        }

        return amount;
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