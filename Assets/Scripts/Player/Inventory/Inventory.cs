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
}

public class InventorySlot
{
    private int _count = 0;
    private Item _item = null;

    public int Count => _count;
    public Item Item => _item;

    public void Use()
    {
        if (_item is UsableItem)
        {
            ((UsableItem)_item).Use();
            if (((UsableItem)_item).DestroyOnUse)
            {
                _count--;
                if (_count == 0)
                {
                    _item = null;
                }
            }
            return;
        }

        if (_item is EquipmentItem)
        {
            ((EquipmentItem)_item).Equip();
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