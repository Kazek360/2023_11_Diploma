using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemScriptableObject", menuName = "ScriptableObjects/Items/Item")]
public class ItemScriptableObject : ScriptableObject
{
    public enum ItemType
    {
        EQUIPMENT, CONSUMABLE, REGULAR
    }
    
    [SerializeField] private int itemID;
    [SerializeField] private string itemName;
    [SerializeField][TextArea] protected string itemDesc;
    [Tooltip("Multiple, because of imagination system")][SerializeField] private List<Sprite> itemSprites;
    [Tooltip("How many of this item can be in a stack")][SerializeField] private int maxStackSize;
    [SerializeField] private ItemType itemType;
    
    [Header("Equipment")]
    [SerializeField] private List<ItemActionContainer> equip;
    [SerializeField] private List<ItemActionContainer> unequip;
    [SerializeField] private EquipmentSlotType equipmentSlotType;
    [Tooltip("Applied when equipped")][SerializeField] private List<StatModifierContainer> modifiers;
    [Tooltip("Applied per level of imagination")][SerializeField] private List<StatModifierContainer> imaginationModifiers;
    
    [Header("Consumable")]
    [SerializeField] private List<ItemEffectContainer> use;
    [SerializeField] private bool destroyOnUse;
}
