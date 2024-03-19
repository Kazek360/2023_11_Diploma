using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityDisplaySummaryScript : MonoBehaviour
{
    [SerializeField] private Image Image;
    [SerializeField] private TextMeshProUGUI Nameplate;
    [SerializeField] private TextMeshProUGUI HPdisplay;
    
    public void SetValues(IHittable hittable)
    {
        Image.sprite = hittable.GetImage();
        Nameplate.text = hittable.GetEntityName();
        HPdisplay.text = hittable.GetCurrentHealth() + "/" + hittable.GetMaxHealth() + " HP";
    }
}
