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
    private EquipmentSlot[] _equipmentSlots;
    private Inventory()
    {
        _itemLibrary = ItemLibrary.GetInstance();
        _inventorySlots = new InventorySlot[7,5];
        _equipmentSlots = new[] { 
            new EquipmentSlot(EquipmentSlotType.feet),
            new EquipmentSlot(EquipmentSlotType.hand),
            new EquipmentSlot(EquipmentSlotType.head),
            new EquipmentSlot(EquipmentSlotType.legs),
            new EquipmentSlot(EquipmentSlotType.torso)
        };
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                _inventorySlots[i, j] = new InventorySlot();
            }
        }
        // AddItem(0, 2);
        // AddItem(0, 10);
        // AddItem(2, 1);
        // AddItem(1, 1);
        // AddItem(3, 1);
    }

    public InventorySlot GetSlot(int x, int y)
    {
        return _inventorySlots[x, y];
    }

    public EquipmentSlot GetEquipmentSlot(EquipmentSlotType slotType)
    {
        foreach (EquipmentSlot slot in _equipmentSlots)
        {
            if (slot.SlotType == slotType)
            {
                return slot;
            }
        }
        return null;
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
        
        foreach (InventorySlot inventorySlot in _inventorySlots)
        {
            if (amount == 0)
            {
                break;
            }
            if (inventorySlot.Item is null)
            {
                int toAdd = Math.Min(amount, item.MaxStackSize);
                inventorySlot.AddItem(item, toAdd);
                amount -= toAdd;
                continue;
            }

            if (inventorySlot.Item.ItemID == itemID)
            {
                int toAdd = Math.Min(amount, item.MaxStackSize - inventorySlot.Count);
                inventorySlot.AddItem(item, toAdd);
                amount -= toAdd;
                continue;
            }
        }

        return amount;
    }
}

