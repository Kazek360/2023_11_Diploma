

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
            "Dobre do picia", 
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
            delegate { Debug.Log("MIECZ ODŁOŻONY DO EKWIPUNKU"); } ,
            EquipmentSlotType.hand
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