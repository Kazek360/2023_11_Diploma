using System.Collections.Generic;
using Shared;
using UnityEngine;

/**
 * ItemLibrary is used to instantiate predefined items
 */
public class ItemLibrary : Singleton<ItemLibrary>
{
    private Dictionary<string, ItemScriptableObject> _items;
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
    public ItemScriptableObject GetItem(string id)
    {
        return _items[id];
    }
    private Dictionary<string, ItemScriptableObject> CreateItemDictionary()
    {
        var allItems = Resources.LoadAll<ItemScriptableObject>("Items");
        var idToDictionary = new Dictionary<string, ItemScriptableObject>();
        foreach (var item in allItems)
        {
            if (idToDictionary.ContainsKey(item.name))
            {
                Debug.LogWarning($"Found Duplicate ID: {item.name}");
            }
            idToDictionary.Add(item.name, item);
        }

        return idToDictionary;
    }
}