using System;
using System.Collections.Generic;
using System.Text;
using Skill;
using Skill.Managers;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemScriptableObject", menuName = "ScriptableObjects/Items/Item")]
public class ItemScriptableObject : ScriptableObject
{
    public enum ItemType
    {
        EQUIPMENT, CONSUMABLE, REGULAR
    }
    
    
    [SerializeField] public string itemName;
    [SerializeField][TextArea] protected string itemDesc;
    [Tooltip("Multiple, because of imagination system")][SerializeField] private List<Sprite> itemSprites;
    [Tooltip("How many of this item can be in a stack")][SerializeField] public int maxStackSize;
    [SerializeField] public ItemType itemType;
    
    [Header("Equipment")]
    [SerializeField] private List<ItemActionContainer> equip;
    [SerializeField] private List<ItemActionContainer> unequip;
    [SerializeField] public EquipmentSlotType equipmentSlotType;
    [Tooltip("Applied when equipped")][SerializeField] private List<ItemStatModifier> modifiers;
    [Tooltip("Applied per level of imagination")][SerializeField] private List<ItemStatModifier> imaginationModifiers;
    
    [Header("Consumable")]
    [SerializeField] private List<ItemEffectContainer> use;
    [SerializeField] private List<ItemActionContainer> onUse;
    [SerializeField] public bool destroyOnUse;
    
    public Sprite ItemSprite()
    {
        return itemSprites[0];
    }
    
    public Sprite ItemSprite(int imaginationLevel)
    {
        return itemSprites[Math.Min(imaginationLevel, itemSprites.Count - 1)];
    }
    
    public string ItemDesc()
    {
        if (itemType == ItemType.EQUIPMENT)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(itemDesc + "\n");
            foreach (var modifier in modifiers)
            {
                builder.Append(modifier.StatString() + "\n");
            }
            builder.Append("Na jednostkę wyobraźni:\n");
            foreach (var modifier in imaginationModifiers)
            {
                builder.Append(modifier.StatString() + "\n");
            }
            return builder.ToString();
        }
        
        return itemDesc;
    }
    
    public void Equip()
    {
        if (itemType != ItemType.EQUIPMENT)
        {
            return;
        }
        foreach (var modifier in modifiers)
        {
            if (SkillTreeManager.GetNodeByName("IMAGINATION_5_1").state == SkillTreeNode.State.Activated)
            {
                var multModifier = new ItemStatModifier(modifier.StatType, (int)(modifier.Modifier * 1.3f), modifier.ItemID);
                PlayerStats.GetInstance().AddItemModifier(multModifier);
            }
            else
            {
                PlayerStats.GetInstance().AddItemModifier(modifier);
            }
        }

        foreach (ItemActionContainer container in equip)
        {
            container.Trigger();
        }
    }

    public void ApplyFavorite(float modifierVal)
    {
        if (itemType != ItemType.EQUIPMENT)
        {
            return;
        }
        foreach (var modifier in modifiers)
        {
            var multModifier = new ItemStatModifier(modifier.StatType, (int)(modifier.Modifier * modifierVal), modifier.ItemID);
            PlayerStats.GetInstance().AddItemModifier(multModifier);
        }
    }

    public void Unequip()
    {
        if (itemType != ItemType.EQUIPMENT)
        {
            return;
        }

        foreach (ItemActionContainer container in unequip)
        {
            container.Trigger();
        }
    }
    
    public void ChangeImagination(int currImagination)
    {
        PlayerStats.GetInstance().ItemRemoved(name);
        foreach (var modifier in modifiers)
        {
            PlayerStats.GetInstance().AddItemModifier(modifier);
        }

        for (int i = 0; i < currImagination; i++)
        {
            foreach (var modifier in imaginationModifiers)
            {
                PlayerStats.GetInstance().AddItemModifier(modifier);
            }
        }
    }

    public void Use()
    {
        foreach (ItemEffectContainer container in use)
        {
            container.ApplyEffect();
        }
        foreach (ItemActionContainer container in onUse)
        {
            container.Trigger();
        }
    }
}
