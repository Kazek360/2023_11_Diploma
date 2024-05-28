using System;
using SaveSystem;
using SaveSystem.Data;
using UnityEngine;

public class Inventory : MonoBehaviour, IDataPersistence
{
    private ItemLibrary _itemLibrary;
    
    private InventorySlot[,] _inventorySlots;
    private EquipmentSlot[] _equipmentSlots;
    public void OnEnable()
    {
        GameConfig.PlayerInventory = this;
        _itemLibrary = ItemLibrary.GetInstance();
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
            Load();
        }
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
            }
        }

        return amount;
    }

    public void Load()
    {
        AddItem(2, 1);
        AddItem(1, 1);
        AddItem(4, 1);
    }

    public void LoadData(GameData gameData)
    {
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
        
        for (var i = 0; i < gameData.inventory.GetLength(0); i++)
        {
            for (var j = 0; j < gameData.inventory.GetLength(1); j++)
            {
                if (gameData.inventory[i, j] is not null)
                {
                    AddItem(gameData.inventory[i, j].Item1, gameData.inventory[i, j].Item2, i, j);
                }
            }
        }

        for (int i = 0; i < gameData.equipment.GetLength(0); i++)
        {
            _equipmentSlots[i].EquipItem((EquipmentItem)ItemLibrary.GetInstance().GetItem(gameData.equipment[i]));
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
                    gameData.inventory[i, j] =
                        new Tuple<int, int>(_inventorySlots[i, j].Item.ItemID, _inventorySlots[i, j].Count);
                }
                else
                {
                    gameData.inventory[i, j] = null;
                }
            }
        }

        for (var i = 0; i < _equipmentSlots.Length; i++)
        {
            if (_equipmentSlots[i].Item is not null)
            {
                gameData.equipment[i] = _equipmentSlots[i].Item.ItemID;
            }
            else
            {
                gameData.equipment[i] = -1;
            }
        }
    }
}

