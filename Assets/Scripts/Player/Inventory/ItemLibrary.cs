using System.Collections.Generic;
using Shared;
using UnityEngine;

/**
 * ItemLibrary is used to instantiate predefined items
 */
public class ItemLibrary : Singleton<ItemLibrary>
{
    private Dictionary<int, ItemScriptableObject> _items;
    protected override void Awake()
    {
        base.Awake();
        _items = CreateItemDictionary();
    }
    
    /**
     * Get a reference to item object
     *
     * <param name="id">ID of desired item</param>
     * <returns>Requested item object</returns>
     */
    public ItemScriptableObject GetItem(int id)
    {
        return _items[id];
    }
    private Dictionary<int, ItemScriptableObject> CreateItemDictionary()
    {
        var allItems = Resources.LoadAll<ItemScriptableObject>("Items");
        var idToDictionary = new Dictionary<int, ItemScriptableObject>();
        foreach (var item in allItems)
        {
            if (idToDictionary.ContainsKey(item.itemID))
            {
                Debug.LogWarning($"Found Duplicate ID: {item.name}");
            }

            idToDictionary.Add(item.itemID, item);
        }

        return idToDictionary;
    }
}