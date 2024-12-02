using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCard : MonoBehaviour, ITooltippable
{
    private ItemScriptableObject itemObject;
    [SerializeField] private Image ItemImage;
    [SerializeField] private TextMeshProUGUI ItemNameField;
    [SerializeField] private TextMeshProUGUI ItemCountField;
    private int _itemOrderInEntityList;
    private CombatUI _controllerClass;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SetItem(ItemScriptableObject itemObject)
    {
        this.itemObject = itemObject;
        ItemNameField.text = itemObject.itemName;
        ItemCountField.text = ItemCount() + "";
        ItemImage.sprite = itemObject.ItemSprite();
    }
    
    /**
     * Sets 'controller' - CombatUI class within which this object is
     *
     * <param name="newController">Controller for this object</param>
     */
    public void SetController(CombatUI newController)
    {
        _controllerClass = newController;
    }

    /**
     * If clicked, notifies controller about it
     */
    public void OnClicked()
    {
        Debug.Log("ITEM USED");
        itemObject.Use();
        GameConfig.PlayerInventory.RemoveItem(itemObject.itemID, 1);
        ItemCountField.text = ItemCount() + "";
        if (ItemCount() <= 0)
        {
            gameObject.SetActive(false);
        }
        _controllerClass.OnItemConsumed();
    }

    public int ItemCount()
    {
        return GameConfig.PlayerInventory.CountItems(itemObject.itemID);
    }
    
    public string GetTooltipHeader()
    {
        return itemObject.itemName;
    }

    public string GetTooltipDesc()
    {
        return itemObject.ItemDesc();
    }
}
