using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCard : MonoBehaviour, ITooltippable
{
    private UsableItem itemObject;
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
    
    public void SetItem(UsableItem itemObject, int orderNumber)
    {
        this.itemObject = itemObject;
        ItemNameField.text = itemObject.ItemName;
        ItemCountField.text = ItemCount() + "";
        ItemImage.sprite = itemObject.ItemSprite();
        _itemOrderInEntityList = orderNumber;
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
        itemObject.Use();
        ItemCountField.text = ItemCount() + "";
        _controllerClass.OnItemConsumed();
    }

    public int ItemCount()
    {
        return GameConfig.PlayerInventory.CountItems(itemObject.ItemID);
    }
    
    public string GetTooltipHeader()
    {
        return itemObject.ItemName;
    }

    public string GetTooltipDesc()
    {
        return itemObject.ItemDesc();
    }
}
