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
            "PIWO PJATK leczy 1 HP", 
            Resources.Load<Sprite>("ItemSprites/piwo"), 
            10,
            delegate
            {
                Debug.Log("Piwo wypite"); 
                PlayerStats.GetInstance().ModifyWitHealth(1);
            },
            true
        ));
        
        _items.Add(new EquipmentItem(
            itemID: 2, 
            "Scyzoryk", 
            "Można nim machać. Pozwala na użycie  zdolności krótkodystansowej Scyzoryk", 
            Resources.Load<Sprite>("ItemSprites/Sword1"), 
            delegate
            {
                UnlockableAbility slingshot = new UnlockableAbility();
                slingshot.AbilitySO = GameConfig.abilityLibrary.GetAbility(12);
                slingshot.Recipient = UnlockableAbility.PlayerType.WIT;
                UnlockedAbilities.GetInstance().UnlockAbility(slingshot);
            },
            delegate
            {
                UnlockableAbility slingshot = new UnlockableAbility();
                slingshot.AbilitySO = GameConfig.abilityLibrary.GetAbility(12);
                slingshot.Recipient = UnlockableAbility.PlayerType.WIT;
                UnlockedAbilities.GetInstance().RemoveAbility(slingshot);
            }, 
            EquipmentSlotType.hand ,
            new List<ItemStatModifier>()
            {
                new ItemStatModifier(StatType.WitStrength, 2, 2)
            }
        ));
        
        _items.Add(new EquipmentItem(
            itemID: 3, 
            "Kamień do rzucania", 
            "Daj kamienia", 
            Resources.Load<Sprite>("ItemSprites/Heart1"), 
            delegate { }, 
            delegate {  }, 
            EquipmentSlotType.hand,
            new List<ItemStatModifier>()
            {
                new ItemStatModifier(StatType.WitAgility, 1, 3)
            }
        ));
        
        _items.Add(new EquipmentItem(
            itemID: 4, 
            "Proca", 
            "Brzydki kuzyn łuku. Pozwala na używanie zdolności dystansowej Strzał", 
            Resources.Load<Sprite>("ItemSprites/Heart1"),
            delegate
            {
                UnlockableAbility slingshot = new UnlockableAbility();
                slingshot.AbilitySO = GameConfig.abilityLibrary.GetAbility(11);
                slingshot.Recipient = UnlockableAbility.PlayerType.WIT;
                UnlockedAbilities.GetInstance().UnlockAbility(slingshot);
            },
            delegate
            {
                UnlockableAbility slingshot = new UnlockableAbility();
                slingshot.AbilitySO = GameConfig.abilityLibrary.GetAbility(11);
                slingshot.Recipient = UnlockableAbility.PlayerType.WIT;
                UnlockedAbilities.GetInstance().RemoveAbility(slingshot);
            }, 
            EquipmentSlotType.hand,
            new List<ItemStatModifier>()
            {
                new ItemStatModifier(StatType.WitAgility, 1, 3)
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