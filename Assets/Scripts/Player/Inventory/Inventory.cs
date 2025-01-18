using System;
using System.Collections.Generic;
using SaveSystem;
using SaveSystem.Data;
using UnityEngine;

/**
 * Player inventory script
 *
 * Allows for equipping items, dropping and picking them up, as well as other interactions
 */
public class Inventory : MonoBehaviour, IDataPersistence
{
    private InventorySlot[,] _inventorySlots;
    private EquipmentSlot[] _equipmentSlots;
    public void OnEnable()
    {
        GameConfig.PlayerInventory = this;
        _inventorySlots = new InventorySlot[7,5];
        _equipmentSlots = new[] { 
            new EquipmentSlot(EquipmentSlotType.feet),
            new EquipmentSlot(EquipmentSlotType.hand),
            new EquipmentSlot(EquipmentSlotType.head),
            new EquipmentSlot(EquipmentSlotType.legs),
            new EquipmentSlot(EquipmentSlotType.torso)
        };
        for (int i = 0; i < _inventorySlots.GetLength(0); i++)
        {
            for (int j = 0; j < _inventorySlots.GetLength(1); j++)
            {
                _inventorySlots[i, j] = new InventorySlot();
            }
        }

        if (_inventorySlots[0,0].Item is null)
        {
            LoadDefault();
        }
    }

    /**
     * Get a slot in given coordinates of inventory grid
     *
     * <param name="x">X coordinate</param>
     * <param name="y">Y coordinate</param>
     */
    public InventorySlot GetSlot(int x, int y)
    {
        return _inventorySlots[x, y];
    }

    /**
     * Get an equipment slot for given equipment type
     *
     * <param name="slotType">Equipment slot type</param>
     */
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

    /**
     * Use item on given position in inventory
     *
     * <param name="x">X coordinate in inventory grid</param>
     * <param name="y">Y coordinate in inventory grid</param>
     */
    public void UseItem(int x, int y)
    {
        _inventorySlots[x, y].Use();
    }

    /**
     * Drop multiple of an item on given position in inventory
     *
     * <param name="x">X coordinate in inventory grid</param>
     * <param name="y">Y coordinate in inventory grid</param>
     * <param name="amount">How many instances of said item are to be dropped</param>
     */
    public void DropItem(int x, int y, int amount)
    {
        _inventorySlots[x, y].RemoveItem(amount);
    }
    
    /**
     * Drop item on given position in inventory
     *
     * <param name="x">X coordinate in inventory grid</param>
     * <param name="y">Y coordinate in inventory grid</param>
     */
    public void DropItem(int x, int y)
    {
        _inventorySlots[x, y].RemoveItem();
    }
    
    public int RemoveItem(string id, int amountToRemove)
    {
        var item = ItemLibrary.Instance.GetItem(id);
        
        foreach (var inventorySlot in _inventorySlots)
        {
            if (amountToRemove == 0)
            {
                return amountToRemove;
            }
            if (inventorySlot.Item is null)
            {
                continue;
            }

            if (inventorySlot.Item.name.Equals(id))
            {
                int removeNow = Math.Min(amountToRemove, inventorySlot.Count);
                inventorySlot.RemoveItem(removeNow);
                amountToRemove -= removeNow;
            }
        }
        return amountToRemove;
    }

    /**
     * Place items in given position in inventory
     *
     * <param name="id">ID of the desired item</param>
     * <param name="amount">Amount of the desired item</param>
     * <param name="x">X coordinate in inventory grid</param>
     * <param name="y">Y coordinate in inventory grid</param>
     */
    public void AddItem(string id, int amount, int x, int y)
    {
        bool success = _inventorySlots[x, y].AddItem(ItemLibrary.Instance.GetItem(id), amount);
        if (!success)
        {
            throw new Exception("DODANIE PRZEDMIOTU NIE POWIODŁO SIĘ");
        }
    }
    
    /**
     * Add multiple of a given item to inventory
     * Generally used in most cases of item acquisition
     *
     * <param name="id">ID of an item to be added</param>
     * <param name="amount">Amount of an item to be added</param>
     * <returns>Amount of items which the function was unable to add</returns>
     */
    public int AddItem(string id, int amount)
    {
        var item = ItemLibrary.Instance.GetItem(id);
        
        foreach (var inventorySlot in _inventorySlots)
        {
            if (amount == 0)
            {
                break;
            }
            if (inventorySlot.Item is null)
            {
                int toAdd = Math.Min(amount, item.maxStackSize);
                inventorySlot.AddItem(item, toAdd);
                amount -= toAdd;
                continue;
            }

            if (inventorySlot.Item.name.Equals(id))
            {
                int toAdd = Math.Min(amount, item.maxStackSize - inventorySlot.Count);
                inventorySlot.AddItem(item, toAdd);
                amount -= toAdd;
            }
        }

        return amount;
    }

    /**
     * Adds the default inventory items
     */
    private void LoadDefault()
    {
        AddItem("beer", 2);
        AddItem("knife", 1);
    }

    public void LoadData(GameData gameData)
    {
        if (DataPersistenceManager.Instance.CurrentLoadMode == LoadMode.NewGame)
        {
            return;
        }
        _inventorySlots = new InventorySlot[7,5];
        _equipmentSlots = new[] { 
            new EquipmentSlot(EquipmentSlotType.feet),
            new EquipmentSlot(EquipmentSlotType.hand),
            new EquipmentSlot(EquipmentSlotType.head),
            new EquipmentSlot(EquipmentSlotType.legs),
            new EquipmentSlot(EquipmentSlotType.torso)
        };
        for (int i = 0; i < _inventorySlots.GetLength(0); i++)
        {
            for (int j = 0; j < _inventorySlots.GetLength(1); j++)
            {
                _inventorySlots[i, j] = new InventorySlot();
            }
        }
        
        for (var i = 0; i < gameData.inventory.items.GetLength(0); i++)
        {
            for (var j = 0; j < gameData.inventory.items.GetLength(1); j++)
            {
                if (gameData.inventory.items[i, j] is not null)
                {
                    AddItem(gameData.inventory.items[i, j].Item1, gameData.inventory.items[i, j].Item2, i, j);
                }
            }
        }

        for (int i = 0; i < gameData.inventory.equipment.GetLength(0); i++)
        {
            if (!gameData.inventory.equipment[i].Equals(""))
            {
                _equipmentSlots[i].EquipItem(ItemLibrary.Instance.GetItem(gameData.inventory.equipment[i]));
            }
        }
    }

    public void SaveData(ref GameData gameData)
    {
        for (var i = 0; i < _inventorySlots.GetLength(0); i++)
        {
            for (var j = 0; j < _inventorySlots.GetLength(1); j++)
            {
                if (_inventorySlots[i, j].Item is not null)
                {
                    gameData.inventory.items[i, j] =
                        new Tuple<string, int>(_inventorySlots[i, j].Item.name, _inventorySlots[i, j].Count);
                }
                else
                {
                    gameData.inventory.items[i, j] = null;
                }
            }
        }

        for (var i = 0; i < _equipmentSlots.Length; i++)
        {
            if (_equipmentSlots[i].Item is not null)
            {
                gameData.inventory.equipment[i] = _equipmentSlots[i].Item.name;
            }
            else
            {
                gameData.inventory.equipment[i] = "";
            }
        }
    }

    public int CountItems(string id)
    {
        int result = 0;
        foreach (var slot in _inventorySlots)
        {
            if (slot.Item is not null)
            {
                if (slot.Item.name.Equals(id))
                {
                    result += slot.Count;
                }
            }
        }

        return result;
    }

    public HashSet<string> GetItems(ItemScriptableObject.ItemType type)
    {
        var result = new HashSet<string>();
        foreach (var slot in _inventorySlots)
        {
            if (slot.Item is not null)
            {
                if (slot.Item.itemType == type)
                {
                    result.Add(slot.Item.name);
                }
            }
        }

        return result;
    }
}

