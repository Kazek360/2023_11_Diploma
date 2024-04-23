using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image ItemImage;
    [SerializeField] private  TextMeshProUGUI Count;
    [SerializeField] private int x, y;
    

    public int GetCoordinateX()
    {
        return x;
    }
    
    public int GetCoordinateY()
    {
        return y;
    }

    public void UpdateSlot(Sprite sprite, int amount)
    {
        if (sprite is null)
        {
            ItemImage.enabled = false;
        }
        else
        {
            ItemImage.enabled = true;
            ItemImage.sprite = sprite;
        }
        if (amount == 0)
        {
            Count.enabled = false;
            return;
        }
        Count.enabled = true;
        Count.text = "" + amount;
        
    }
}
