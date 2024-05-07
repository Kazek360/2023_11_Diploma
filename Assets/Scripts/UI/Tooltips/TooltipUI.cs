using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Header;
    [SerializeField] private TextMeshProUGUI Desc;
    private float width, height;

    public void Setup(string header,  string desc)
    {
        Header.text = header;
        Desc.text = desc;
        gameObject.transform.SetParent(GameConfig.canvas.gameObject.transform);
        width = GetComponent<RectTransform>().rect.width;
        height = GetComponent<RectTransform>().rect.height;
        SetupPos();
    }

    private void SetupPos()
    {
        float x, y;
        if (Input.mousePosition.y < Screen.height - height)
        {
            y = Input.mousePosition.y + height / 2;
        }
        else
        {
            y = Input.mousePosition.y - height / 2;
        }

        if (Input.mousePosition.x < Screen.width - width)
        {
            x = Input.mousePosition.x + width / 2;
        }
        else
        {
            x = Input.mousePosition.x - width / 2;
        }
        
        gameObject.transform.position = new Vector2(x, y);
    }

    public void Delete()
    {
        Destroy(gameObject);
    }
}
