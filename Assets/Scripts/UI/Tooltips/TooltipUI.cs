using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Header;
    [SerializeField] private TextMeshProUGUI Desc;

    public void Setup(string header,  string desc)
    {
        Header.text = header;
        Desc.text = desc;
    }

    public void Delete()
    {
        Destroy(gameObject);
    }
}
