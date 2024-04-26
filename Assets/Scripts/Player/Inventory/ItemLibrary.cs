

using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemLibrary
{
    private static ItemLibrary _instance;
    private static List<Item> _items;

    public static ItemLibrary GetInstance()
    {
        if (_instance is null)
        {
            _instance = new ItemLibrary();
        }
        return _instance;
    }

    private ItemLibrary()
    {
        _items = new List<Item>();
        _items.Add(new Item(
            itemID: 0, 
            "Kamień", 
            "Przydatny dla jaskiniowców", 
            Resources.Load<Sprite>("ItemSprites/Heart1"), 
            10
            ));
        
        _items.Add(new UsableItem(
            itemID: 1, 
            "Piwo", 
            "PIWO PJATK", 
            Resources.Load<Sprite>("ItemSprites/piwo"), 
            10,
            delegate { Debug.Log("Piwo wypite"); },
            true
        ));
        
        _items.Add(new EquipmentItem(
            itemID: 2, 
            "Miecz", 
            "Można nim machać", 
            Resources.Load<Sprite>("ItemSprites/Sword1"), 
            delegate { Debug.Log("MIECZ WYEKWIPOWANY"); }, 
            EquipmentSlotType.hand ,
            new List<ItemStatModifier>()
            {
                new ItemStatModifier(StatType.Strength, 2, 2)
            }
        ));
        
        _items.Add(new EquipmentItem(
            itemID: 3, 
            "Kamień do rzucania", 
            "Daj kamienia", 
            Resources.Load<Sprite>("ItemSprites/Heart1"), 
            delegate { Debug.Log("KAMIEŃ WYEKWIPOWANY"); }, 
            EquipmentSlotType.hand,
            new List<ItemStatModifier>()
            {
                new ItemStatModifier(StatType.Agility, 1, 3)
            }
        ));
    }

    public Item GetItem(int id)
    {
        foreach (Item item in _items)
        {
            if (item.ItemID == id)
            {
                return item;
            }
        }

        return null;
    }
}