using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetCard : MonoBehaviour
{
    private IHittable _target;
    [SerializeField] private GameObject targetNameField;
    [SerializeField] private GameObject targetImage;
    private void OnEnable()
    {
    }

    // Start is called before the first frame update
    public void SetTarget(IHittable script)
    {
        _target = script;
        targetNameField.GetComponent<TextMeshProUGUI>().text = _target.GetEntityName();
        targetImage.GetComponent<Image>().sprite = _target.GetImage();
    }

    public IHittable GetTargetScript()
    {
        return _target;
    }
}
