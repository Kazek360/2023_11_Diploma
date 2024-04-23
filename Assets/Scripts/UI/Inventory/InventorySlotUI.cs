using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image ItemImage;
    [SerializeField] private int x, y;
    

    public void SetItemImageSprite(Sprite sprite)
    {
        ItemImage.sprite = sprite;
    }

    public int GetCoordinateX()
    {
        return x;
    }
    
    public int GetCoordinateY()
    {
        return y;
    }
}
