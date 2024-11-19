using System.Collections.Generic;
using UnityEngine;

/**
 * ItemLibrary is used to instantiate predefined items
 * Will soon be replaced with Resources based SOs
 */
public class ItemLibrary: MonoBehaviour
{
    [SerializeField] private List<ItemScriptableObject> items;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (GameConfig.itemLibrary is null)
        {
            GameConfig.itemLibrary = this;
        }
    }


    /**
     * Get a reference to item object
     *
     * <param name="id">ID of desired item</param>
     * <returns>Requested item object</returns>
     */
    public ItemScriptableObject GetItem(int id)
    {
        foreach (ItemScriptableObject item in items)
        {
            if (item.itemID == id)
            {
                return item;
            }
        }

        return null;
    }
}